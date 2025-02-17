using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI; // UI�p
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    private SceneChange sceneChange;
    private enum Mode
    {
        BackSelect,
        Retry
    }

    private Mode _nowSelectMode = Mode.BackSelect;

    [SerializeField]
    private Image _arrowImage;
    [SerializeField]
    private GameObject[] texts;

    [SerializeField, Header("�������̃L�[�R��")]
    private InputAction _leftAction;

    [SerializeField, Header("�E�����̃L�[�R��")]
    private InputAction _RightAction;

    [SerializeField, Header("�I���̃L�[�R��")]
    private InputAction _selectAction;

    public Text backSelectText;
    public Text retryText;

    public PostProcessVolume postProcessVolume;
    private ColorGrading colorGrading;
    private float targetSaturation = 0;
    private float fadeSpeed = 1.5f;

    public PlayerController_2D player;
    private bool isGameOver = false;

    public Text gameOverText;

    private Animator playerAnimator;

    private void OnEnable()
    {
        _leftAction?.Enable();
        _RightAction?.Enable();
        _selectAction?.Enable();
    }

    private void OnDisable()
    {
        _leftAction?.Disable();
        _RightAction?.Disable();
        _selectAction?.Disable();
    }

    private void Awake()
    {

        if (postProcessVolume.profile.TryGetSettings(out ColorGrading cg))
        {
            colorGrading = cg;
        }

        if (gameOverText != null && backSelectText != null && retryText != null && _arrowImage != null)
        {
            gameOverText.gameObject.SetActive(false);
            backSelectText.gameObject.SetActive(false);
            retryText.gameObject.SetActive(false);
            _arrowImage.gameObject.SetActive(false);
        }


        if (player == null)
        {
            Debug.LogError("Player�X�N���v�g�����蓖�Ă��Ă��܂���I");
        }
        else
        {
            StartCoroutine(LateStart());
        }

        sceneChange = FindAnyObjectByType<SceneChange>();
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



        if (_leftAction.WasPressedThisFrame())
        {
            _nowSelectMode--;
            if (_nowSelectMode < 0)
            {
                _nowSelectMode = Mode.Retry;
            }

            _arrowImage.gameObject.transform.position = texts[(int)_nowSelectMode].transform.position;
        }


        if (_RightAction.WasPressedThisFrame())
        {
            _nowSelectMode++;
            if (_nowSelectMode > Mode.Retry)
            {
                _nowSelectMode = Mode.BackSelect;
            }
            _arrowImage.gameObject.transform.position = texts[(int)_nowSelectMode].transform.position;
        }

        if (_selectAction.WasPressedThisFrame())
        {
            switch (_nowSelectMode)
            {
                case Mode.BackSelect:
                    SceneManager.LoadScene("SelectScene");
                    break;

                case Mode.Retry:
                    Debug.Log("���g���C");
                    // ���݂�Scene���擾
                    Scene loadScene = SceneManager.GetActiveScene();
                    // ���݂̃V�[�����ēǂݍ��݂���
                    SceneManager.LoadScene(loadScene.name);
                    break;

            }
        }
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        playerAnimator = player.Animator;

        if (playerAnimator == null)
        {
            Debug.LogError("Player��Animator�����蓖�Ă��Ă��܂���I");
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        targetSaturation = (targetSaturation == 0) ? -100 : 0;

        if (gameOverText != null && backSelectText != null && retryText != null && _arrowImage != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "GAME OVER";
            backSelectText.gameObject.SetActive(true);
            retryText.gameObject.SetActive(true);
            _arrowImage.gameObject.SetActive(true);
        }

        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("GameOver");
        }
        else
        {
            StartCoroutine(LateStart());
        }

        // �v���C���[�̓������~
        player.enabled = false;
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
    }
}
