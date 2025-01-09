using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkybox : MonoBehaviour
{
    public Material skyboxMaterial;
    public float duration = 2f; // フェード時間

    void OnEnable()
    {
        Debug.Log("スカイボックス");

        StartFade(0f, 1f);
    }

    private void Start()
    {
        skyboxMaterial.SetFloat("_value", 0f);
    }

    public void StartFade(float from, float to)
    {
        // 初期値を設定
        skyboxMaterial.SetFloat("_value", from);

        // DOTweenで値を補間
        DOTween.To(
            () => skyboxMaterial.GetFloat("_value"),
            x => skyboxMaterial.SetFloat("_value", x),
            to,
            duration
        ).OnUpdate(() =>
        {
            // 必要に応じてリアルタイムの環境更新（オプション）
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
