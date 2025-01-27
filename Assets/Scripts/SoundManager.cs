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
        // インスタンスが既に存在する場合は、重複を防ぐ
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // インスタンスを設定
        Instance = this;
        // BGMの自動再生
        DontDestroyOnLoad(gameObject); // シーンをまたいでもオブジェクトが破棄されないようにする
    }

    // BGMを再生するメソッド
    public void PlayBGM(AudioClip bgmClip)
    {
        _bgmPlayer.clip = bgmClip;
        _bgmPlayer.Play();
    }

    // SEを再生するメソッド
    public void PlaySE(AudioClip seClip)
    {
        _sePlayer.PlayOneShot(seClip);
    }

    // BGMの停止
    public void StopBGM()
    {
        _bgmPlayer.Stop();
    }

    // SEの停止
    public void StopSE()
    {
        _sePlayer.Stop();
    }

    //BGMの音量設定
    public void SetBGMVolume(float volume)
    {
        _bgmPlayer.volume = volume;
    }

    //SEの音量設定
    public void SetSEVolume(float volume)
    {
        _sePlayer.volume = volume;
    }
}

