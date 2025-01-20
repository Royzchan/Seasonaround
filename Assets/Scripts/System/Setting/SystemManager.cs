using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class SystemManager : MonoBehaviour
{
    [SerializeField, Header("ポーズキー")]
    private InputAction _poseAction;
    static bool _hasAction;
    private void OnEnable()
    {
        _poseAction?.Enable();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(!_hasAction)
        {
            _poseAction.started += (InputAction.CallbackContext callback) =>
            {
                SettingManager setting = SettingManager.GetInstance();
                if (setting != null)
                {
                    if (setting.SelectPage != 0)
                    {
                        setting.ChangePage(0);
                    }
                    else
                    {
                        if (setting.gameObject.activeSelf)
                        {
                            StartCoroutine(setting.CloseSetting());
                        }
                        else
                        {
                            setting.gameObject.SetActive(true);
                        }
                    }

                }

            };
            _hasAction = true;
        }
        
    }
}
