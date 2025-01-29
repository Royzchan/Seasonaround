using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI; // UI用

public class GameOverScript : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;
    private ColorGrading colorGrading;
    private float targetSaturation = 0;
    private float fadeSpeed = 1.5f;

    public PlayerController_2D player;
    private bool isGameOver = false;

    public Text gameOverText;

    private Animator playerAnimator;

    void Start()
    {
        if (postProcessVolume.profile.TryGetSettings(out ColorGrading cg))
        {
            colorGrading = cg;
        }

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }

        if (player == null)
        {
            Debug.LogError("Playerスクリプトが割り当てられていません！");
        }
        else
        {
            StartCoroutine(LateStart());
        }
    }

    void Update()
    {
        if (player != null && player.Hp <= 0 && !isGameOver)
        {
            GameOver();
        }

        if (colorGrading != null)
        {
            colorGrading.saturation.value = Mathf.Lerp(colorGrading.saturation.value, targetSaturation, Time.deltaTime * fadeSpeed);
        }
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        playerAnimator = player.Animator;

        if (playerAnimator == null)
        {
            Debug.LogError("PlayerにAnimatorが割り当てられていません！");
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        targetSaturation = (targetSaturation == 0) ? -100 : 0;

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "GAME OVER";
        }

        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("GameOver");
        }

        // プレイヤーの動きを停止
        player.enabled = false;
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
    }
}
