using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BWFade: MonoBehaviour
{
    // ポストプロセスボリュームを指定
    public PostProcessVolume postProcessVolume;
    // Color Gradingエフェクト
    private ColorGrading colorGrading;
    // 目標の彩度
    private float targetSaturation = 0;
    // フェードの速さ
    private float fadeSpeed = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        // PostProcessVolumeからColorGradingの設定を取得
        if (postProcessVolume.profile.TryGetSettings(out ColorGrading cg))
        {
            colorGrading = cg;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // キー入力で白黒を切り替え
        if (Input.GetKeyDown(KeyCode.B))
        {
            targetSaturation = (targetSaturation == 0) ? -100 : 0;
        }

        // 現在のSaturationを目標値にスムーズに近づける
        if (colorGrading != null)
        {
            colorGrading.saturation.value = Mathf.Lerp(colorGrading.saturation.value, targetSaturation, Time.deltaTime * fadeSpeed);
        }
    }
}
