using UnityEngine;
using UnityEngine.UI;

public class ImageMasking : MonoBehaviour
{
    public Image mainImage; // 切り抜かれるUIImage
    public Image maskImage; // 切り抜き用のImage

    private Material cutoutMaterial;
    Canvas canvas;
    void Start()
    {
        // マテリアルを取得または作成
        cutoutMaterial = Instantiate(mainImage.material);
        mainImage.material = cutoutMaterial;

        // マスク用テクスチャを設定
        cutoutMaterial.SetTexture("_MaskTex", maskImage.mainTexture);
        canvas = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        RectTransform mainRect = mainImage.rectTransform;
        RectTransform maskRect = maskImage.rectTransform;

        Vector2 mainSize = mainRect.rect.size;
        Vector2 maskSize = maskRect.rect.size;

        Vector2 offset = (maskRect.anchoredPosition - mainRect.anchoredPosition) / mainSize;
        cutoutMaterial.SetVector("_Pivot",maskImage.rectTransform.pivot);
        cutoutMaterial.SetVector("_MaskOffset", new Vector4(offset.x, offset.y, maskSize.x / mainSize.x, maskSize.y / mainSize.y));
    }
}