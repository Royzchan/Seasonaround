using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningOneTime : MonoBehaviour
{
    public static OpeningOneTime Instance; // シングルトン

    public bool hasWatchedOpening = false;

    private void Awake()
    {
        // 他のインスタンスが存在する場合は削除
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // シーンをまたいでも削除されない
    }
}