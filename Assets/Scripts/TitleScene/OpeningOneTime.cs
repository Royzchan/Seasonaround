using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningOneTime : MonoBehaviour
{
    public static OpeningOneTime Instance; // �V���O���g��

    public bool hasWatchedOpening = false;

    private void Awake()
    {
        // ���̃C���X�^���X�����݂���ꍇ�͍폜
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // �V�[�����܂����ł��폜����Ȃ�
    }
}