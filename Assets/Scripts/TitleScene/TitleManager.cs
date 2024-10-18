using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TitleManager : MonoBehaviour
{
    private SceneChange sceneChange;
    private enum Mode
    {
        Game,
        Setting,
        Operat
    }

    private Mode _nowSelectMode = Mode.Game;

    [SerializeField]
    private Image _arrowImage;
    [SerializeField]
    private GameObject[] texts;

    [SerializeField,Header("上方向のキーコン")]
    private InputAction _upAction;

    [SerializeField, Header("下方向のキーコン")]
    private InputAction _downAction;

    [SerializeField, Header("選択のキーコン")]
    private InputAction _selectAction;

    private void OnEnable()
    {
        _upAction?.Enable();
        _downAction?.Enable();
        _selectAction?.Enable();
    }

    private void OnDisable()
    {
        _upAction?.Disable();
        _downAction?.Disable();
        _selectAction?.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        sceneChange = FindAnyObjectByType<SceneChange>();

    }

    // Update is called once per frame
    void Update()
    {
        if (_upAction.WasPressedThisFrame())
        {
            _nowSelectMode--;
            if (_nowSelectMode < 0)
            {
                _nowSelectMode = Mode.Operat;
            }

            _arrowImage.gameObject.transform.position = texts[(int)_nowSelectMode].transform.position;
        }


        if (_downAction.WasPressedThisFrame())
        {
            _nowSelectMode++;
            if (_nowSelectMode > Mode.Operat)
            {
                _nowSelectMode = Mode.Game;
            }
            _arrowImage.gameObject.transform.position = texts[(int)_nowSelectMode].transform.position;
        }

        if (_selectAction.WasPressedThisFrame())
        {
            switch (_nowSelectMode)
            {
                case Mode.Game:
                    sceneChange.ChangeScene();
                    break;

                case Mode.Setting:
                    Debug.Log("設定");
                    break;
                case Mode.Operat:
                    Debug.Log("操作説明");
                    break;

            }
        }
    }
}
