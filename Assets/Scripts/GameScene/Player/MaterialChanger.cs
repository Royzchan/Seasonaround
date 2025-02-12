using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class MaterialChanger : MonoBehaviour
{
    // �����ւ��p�̃}�e���A�����i�[
    public Material defaultMaterial;
    public Material alternateMaterial;

    // Renderer�̎Q�Ƃ�ێ�
    public Renderer characterRenderer;

    private Material faceMat;

    void Start()
    {
        // Renderer�R���|�[�l���g���擾

        if (characterRenderer == null)
        {
            Debug.LogError("Renderer��������܂���B�I�u�W�F�N�g���m�F���Ă��������I");
        }


        // �����}�e���A����ݒ�
        SetMaterial(defaultMaterial);
    }

    // �}�e���A����؂�ւ���֐�
    public void SetMaterial(Material newMaterial)
    {
        if (characterRenderer != null && newMaterial != null)
        {
            // ���݂̃}�e���A���z����擾
            Material[] materials = characterRenderer.sharedMaterials;
            // �z��̗v�f��ύX
            materials[1] = newMaterial;
            // �X�V�����z��𔽉f
            characterRenderer.sharedMaterials = materials;
        }
    }

    // �A�j���[�V�����C�x���g����Ăяo���֐�
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
