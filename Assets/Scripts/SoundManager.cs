using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _bgmPlayer;
    [SerializeField]
    private AudioSource _sePlayer;

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        // �C���X�^���X�����ɑ��݂���ꍇ�́A�d����h��
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // �C���X�^���X��ݒ�
        Instance = this;
        // BGM�̎����Đ�
        DontDestroyOnLoad(gameObject); // �V�[�����܂����ł��I�u�W�F�N�g���j������Ȃ��悤�ɂ���
    }

    // BGM���Đ����郁�\�b�h
    public void PlayBGM(AudioClip bgmClip)
    {
        _bgmPlayer.clip = bgmClip;
        _bgmPlayer.Play();
    }

    // SE���Đ����郁�\�b�h
    public void PlaySE(AudioClip seClip)
    {
        _sePlayer.PlayOneShot(seClip);
    }

    // BGM�̒�~
    public void StopBGM()
    {
        _bgmPlayer.Stop();
    }

    // SE�̒�~
    public void StopSE()
    {
        _sePlayer.Stop();
    }

    //BGM�̉��ʐݒ�
    public void SetBGMVolume(float volume)
    {
        _bgmPlayer.volume = volume;
    }

    //SE�̉��ʐݒ�
    public void SetSEVolume(float volume)
    {
        _sePlayer.volume = volume;
    }
}

