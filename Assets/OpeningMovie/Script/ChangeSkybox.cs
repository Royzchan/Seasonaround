using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkybox : MonoBehaviour
{
    public Material skyboxMaterial;
    public float duration = 2f; // �t�F�[�h����

    void OnEnable()
    {
        Debug.Log("�X�J�C�{�b�N�X");

        StartFade(0f, 1f);
    }

    private void Start()
    {
        skyboxMaterial.SetFloat("_value", 0f);
    }

    public void StartFade(float from, float to)
    {
        // �����l��ݒ�
        skyboxMaterial.SetFloat("_value", from);

        // DOTween�Œl����
        DOTween.To(
            () => skyboxMaterial.GetFloat("_value"),
            x => skyboxMaterial.SetFloat("_value", x),
            to,
            duration
        ).OnUpdate(() =>
        {
            // �K�v�ɉ����ă��A���^�C���̊��X�V�i�I�v�V�����j
            DynamicGI.UpdateEnvironment();
        }).OnComplete(() =>
        {
            Debug.Log("Fade Complete!");
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
