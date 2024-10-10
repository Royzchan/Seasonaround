using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Start is called before the first frame update
    void Start()
    {
        sceneChange = FindAnyObjectByType<SceneChange>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _nowSelectMode--;
            if (_nowSelectMode < 0)
            {
                _nowSelectMode = Mode.Operat;
            }

            _arrowImage.gameObject.transform.position = texts[(int)_nowSelectMode].transform.position;
        }


        if (Input.GetKeyDown(KeyCode.S))
        {
            _nowSelectMode++;
            if (_nowSelectMode > Mode.Operat)
            {
                _nowSelectMode = Mode.Game;
            }
            _arrowImage.gameObject.transform.position = texts[(int)_nowSelectMode].transform.position;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (_nowSelectMode)
            {
                case Mode.Game:
                    sceneChange.ChangeScene();
                    break;

                case Mode.Setting:
                    Debug.Log("ê›íË");
                    break;
                case Mode.Operat:
                    Debug.Log("ëÄçÏê‡ñæ");
                    break;

            }
        }


    }
}
