using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI; // UI�p

public class GameOverScript : MonoBehaviour
{
    // �|�X�g�v���Z�X�{�����[�����w��
    public PostProcessVolume postProcessVolume;
    // Color Grading�G�t�F�N�g
    private ColorGrading colorGrading;
    // �ڕW�̍ʓx
    private float targetSaturation = 0;
    // �t�F�[�h�̑���
    private float fadeSpeed = 2.0f;

    // �v���C���[�̃X�N���v�g���Q��
    public PlayerController_2D player;
    private bool isGameOver = false;

    // �Q�[���I�[�o�[����UI�\��
    public Text gameOverText;

    // Start is called before the first frame update
    void Start()
    {
        // PostProcessVolume����ColorGrading�̐ݒ���擾
        if (postProcessVolume.profile.TryGetSettings(out ColorGrading cg))
        {
            colorGrading = cg;
        }

        // �Q�[���I�[�o�[��UI���\���ɐݒ�
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }

        // �v���C���[�̎Q�Ƃ��������ݒ肳��Ă��邩�m�F
        if (player == null)
        {
            Debug.LogError("Player�X�N���v�g�����蓖�Ă��Ă��܂���I");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �v���C���[��HP���`�F�b�N���ăQ�[���I�[�o�[�������s��
        if (player != null && player.Hp <= 0 && !isGameOver)
        {
            GameOver();
        }

        // ���݂�Saturation��ڕW�l�ɃX���[�Y�ɋ߂Â���
        if (colorGrading != null)
        {
            colorGrading.saturation.value = Mathf.Lerp(colorGrading.saturation.value, targetSaturation, Time.deltaTime * fadeSpeed);
        }
    }

    // �Q�[���I�[�o�[����
    private void GameOver()
    {
        isGameOver = true;

        targetSaturation = (targetSaturation == 0) ? -100 : 0;

        // ���݂�Saturation��ڕW�l�ɃX���[�Y�ɋ߂Â���
        if (colorGrading != null)
        {
            colorGrading.saturation.value = Mathf.Lerp(colorGrading.saturation.value, targetSaturation, Time.deltaTime * fadeSpeed);
        }

        // �Q�[���I�[�o�[UI��\��
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "GAME OVER";
        }

        // ���̃Q�[���I�[�o�[�����������ɒǉ��\
        Debug.Log("Game Over");
    }
}