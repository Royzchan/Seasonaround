Shader "UI/MaskedCutout"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _MaskTex ("Mask Texture", 2D) = "white" {} // �؂蔲���p�}�X�N�e�N�X�`��
        _Pivot ("Pivot",Vector) = (0.5,0.5,0,0)
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            sampler2D _MaskTex;

            float4 _Pivot;
            float4 _MaskOffset;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
               // UV���W��␳
               float2 maskUV = i.uv;
               // �I�t�Z�b�g�ƃX�P�[����K�p

               maskUV.x = (maskUV.x - _MaskOffset.x) / _MaskOffset.z  + _Pivot.x * (1.0 -1.0 /_MaskOffset.z);
               maskUV.y = (maskUV.y - _MaskOffset.y) / _MaskOffset.w  + _Pivot.y * (1.0 -1.0 /_MaskOffset.w);

               // �}�X�N�e�N�X�`�����T���v�����O
               fixed4 maskColor = tex2D(_MaskTex, maskUV);

               // ���C���e�N�X�`�����T���v�����O
               fixed4 mainColor = tex2D(_MainTex, i.uv);

               // �}�X�N�����𓧖���
               if (maskColor.a > 0.5)
               {
                   mainColor.a = 0;
               }

               return mainColor;
            }
            ENDCG
        }
    }
}