using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class VolumeButtonScript : SettingButtonScript
{
    [SerializeField,Header("右")]
    InputAction _inputRight;
    [SerializeField,Header("左")]
    InputAction _inputLeft;
    [SerializeField,Header("戻る")]
    InputAction _inputReturn;
    [SerializeField, Header("長押し判定になる時間")]
    private float _pressTime = 0.5f;
    private bool _isSelect = false;
    private bool _isLeft = false;
    private bool _isRight = false;
    //エンターボタンが被ってしまったので特別措置
    private bool _isReturnFlag = false;
    [SerializeField, Header("ボリュームスライダー")]
    private Slider _soundSlider;
    float _pressRightTimer = 0f;
    float _pressLeftTimer = 0f;

    void InputEnable()
    {
        _inputRight.Enable();
        _inputReturn.Enable();
        _inputLeft.Enable();
    }
    void InputDisable()
    {
        _inputRight.Disable();
        _inputReturn.Disable();
        _inputLeft.Disable();
    }

    private void OnDisable()
    {
        InputDisable();
        SliderResetScale();
        _soundSlider.gameObject.GetComponentInChildren<Outline>().enabled = false;
        SettingManager.GetInstance().EnableInput();
    }

    public override void SelectedButton()
    {
        SettingManager.GetInstance().DisableInput();
        InputEnable();
        _isSelect = true;
        _soundSlider.gameObject.GetComponentInChildren<Outline>().enabled = true;
        SliderSetScale();
        GetComponent<Outline>().enabled = false;
    }

    private void Update()
    {
        
        if(_isSelect)
        {
            if(_inputRight.WasPressedThisFrame())
            {
                SettingManager.GetInstance()._soundVolume++;
                _isRight = true;
            }
            else if (_isRight)
            {
                _pressRightTimer += Time.unscaledDeltaTime;
                if (_pressRightTimer > _pressTime)
                {
                    SettingManager.GetInstance()._soundVolume++;
                }
                if (_inputRight.WasReleasedThisFrame())
                {
                    _isRight = false;
                    _pressRightTimer = 0;
                }
            }

            if(_inputLeft.WasPressedThisFrame())
            {
                SettingManager.GetInstance()._soundVolume--;
                _isLeft = true;
            }
            else if(_isLeft)
            {
                Debug.Log(_pressLeftTimer);
                _pressLeftTimer += Time.unscaledDeltaTime;
                if(_pressLeftTimer > _pressTime)
                {
                    SettingManager.GetInstance()._soundVolume--;
                }
                if (_inputLeft.WasReleasedThisFrame())
                {
                    _isLeft = false;
                    _pressLeftTimer = 0;
                }
            }


            if (_inputReturn.WasPressedThisFrame())
            {
                _isSelect = false;
                _soundSlider.gameObject.GetComponentInChildren<Outline>().enabled = false;
                InputDisable();
                GetComponent<Outline>().enabled = true;
                SliderResetScale();
                StartCoroutine(ResetInput());
            }
        }
        _soundSlider.value = SettingManager.GetInstance()._soundVolume;
        
    }
    void SliderSetScale()
    {
        var sliderRect = _soundSlider.GetComponent<RectTransform>();
        sliderRect.DOKill();
        sliderRect.DOScale(new Vector3(_zoomInRatio, _zoomInRatio), _zoomInTime).SetUpdate(true);
    }

    void SliderResetScale()
    {
        var sliderRect = _soundSlider.GetComponent<RectTransform>();
        sliderRect.DOKill();
        sliderRect.DOScale(1.0f, _zoomInTime).SetUpdate(true);
    }
    IEnumerator ResetInput()
    {
        //今日のごり押し関数
        //こちらはInputの入力を二つも作ってしまったためにおきた不幸な事故を無理やりなかったことにする処理です
        //直せたら直します
        yield return new WaitForSecondsRealtime(0.1f);
        SettingManager.GetInstance().EnableInput();
    }
}
