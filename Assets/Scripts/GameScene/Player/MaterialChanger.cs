using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class MaterialChanger : MonoBehaviour
{
    // 差し替え用のマテリアルを格納
    public Material defaultMaterial;
    public Material alternateMaterial;

    // Rendererの参照を保持
    public Renderer characterRenderer;

    private Material faceMat;

    void Start()
    {
        // Rendererコンポーネントを取得

        if (characterRenderer == null)
        {
            Debug.LogError("Rendererが見つかりません。オブジェクトを確認してください！");
        }


        // 初期マテリアルを設定
        SetMaterial(defaultMaterial);
    }

    // マテリアルを切り替える関数
    public void SetMaterial(Material newMaterial)
    {
        if (characterRenderer != null && newMaterial != null)
        {
            // 現在のマテリアル配列を取得
            Material[] materials = characterRenderer.sharedMaterials;
            // 配列の要素を変更
            materials[1] = newMaterial;
            // 更新した配列を反映
            characterRenderer.sharedMaterials = materials;
        }
    }

    // アニメーションイベントから呼び出す関数
    public void ChangeToAlternateMaterial()
    {
        Debug.Log(alternateMaterial);
        SetMaterial(alternateMaterial);
    }

    public void ResetToDefaultMaterial()
    {
        SetMaterial(defaultMaterial);
    }

}
