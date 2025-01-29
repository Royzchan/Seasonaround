Shader "UI/MaskedCutout"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _MaskTex ("Mask Texture", 2D) = "white" {} // 切り抜き用マスクテクスチャ
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
               // UV座標を補正
               float2 maskUV = i.uv;
               // オフセットとスケールを適用

               maskUV.x = (maskUV.x - _MaskOffset.x) / _MaskOffset.z  + _Pivot.x * (1.0 -1.0 /_MaskOffset.z);
               maskUV.y = (maskUV.y - _MaskOffset.y) / _MaskOffset.w  + _Pivot.y * (1.0 -1.0 /_MaskOffset.w);

               // マスクテクスチャをサンプリング
               fixed4 maskColor = tex2D(_MaskTex, maskUV);

               // メインテクスチャをサンプリング
               fixed4 mainColor = tex2D(_MainTex, i.uv);

               // マスク部分を透明化
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