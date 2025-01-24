using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI; // UI用

public class GameOverScript : MonoBehaviour
{
    // ポストプロセスボリュームを指定
    public PostProcessVolume postProcessVolume;
    // Color Gradingエフェクト
    private ColorGrading colorGrading;
    // 目標の彩度
    private float targetSaturation = 0;
    // フェードの速さ
    private float fadeSpeed = 2.0f;

    // プレイヤーのスクリプトを参照
    public PlayerController_2D player;
    private bool isGameOver = false;

    // ゲームオーバー時のUI表示
    public Text gameOverText;

    // Start is called before the first frame update
    void Start()
    {
        // PostProcessVolumeからColorGradingの設定を取得
        if (postProcessVolume.profile.TryGetSettings(out ColorGrading cg))
        {
            colorGrading = cg;
        }

        // ゲームオーバーのUIを非表示に設定
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }

        // プレイヤーの参照が正しく設定されているか確認
        if (player == null)
        {
            Debug.LogError("Playerスクリプトが割り当てられていません！");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーのHPをチェックしてゲームオーバー処理を行う
        if (player != null && player.Hp <= 0 && !isGameOver)
        {
            GameOver();
        }

        // 現在のSaturationを目標値にスムーズに近づける
        if (colorGrading != null)
        {
            colorGrading.saturation.value = Mathf.Lerp(colorGrading.saturation.value, targetSaturation, Time.deltaTime * fadeSpeed);
        }
    }

    // ゲームオーバー処理
    private void GameOver()
    {
        isGameOver = true;

        targetSaturation = (targetSaturation == 0) ? -100 : 0;

        // 現在のSaturationを目標値にスムーズに近づける
        if (colorGrading != null)
        {
            colorGrading.saturation.value = Mathf.Lerp(colorGrading.saturation.value, targetSaturation, Time.deltaTime * fadeSpeed);
        }

        // ゲームオーバーUIを表示
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "GAME OVER";
        }

        // 他のゲームオーバー処理をここに追加可能
        Debug.Log("Game Over");
    }
}