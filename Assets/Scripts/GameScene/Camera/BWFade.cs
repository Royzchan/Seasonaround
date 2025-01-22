using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BWFade: MonoBehaviour
{
    // �|�X�g�v���Z�X�{�����[�����w��
    public PostProcessVolume postProcessVolume;
    // Color Grading�G�t�F�N�g
    private ColorGrading colorGrading;
    // �ڕW�̍ʓx
    private float targetSaturation = 0;
    // �t�F�[�h�̑���
    private float fadeSpeed = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        // PostProcessVolume����ColorGrading�̐ݒ���擾
        if (postProcessVolume.profile.TryGetSettings(out ColorGrading cg))
        {
            colorGrading = cg;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �L�[���͂Ŕ�����؂�ւ�
        if (Input.GetKeyDown(KeyCode.B))
        {
            targetSaturation = (targetSaturation == 0) ? -100 : 0;
        }

        // ���݂�Saturation��ڕW�l�ɃX���[�Y�ɋ߂Â���
        if (colorGrading != null)
        {
            colorGrading.saturation.value = Mathf.Lerp(colorGrading.saturation.value, targetSaturation, Time.deltaTime * fadeSpeed);
        }
    }
}
