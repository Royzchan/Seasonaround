using UnityEngine;
using UnityEngine.UI;

public class ImageMasking : MonoBehaviour
{
    public Image mainImage; // �؂蔲�����UIImage
    public Image maskImage; // �؂蔲���p��Image

    private Material cutoutMaterial;
    Canvas canvas;
    void Start()
    {
        // �}�e���A�����擾�܂��͍쐬
        cutoutMaterial = Instantiate(mainImage.material);
        mainImage.material = cutoutMaterial;

        // �}�X�N�p�e�N�X�`����ݒ�
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