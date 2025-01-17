using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    static SettingManager instance;

    private PlayerController_2D _player;

    [SerializeField, Header("右方向のキーコン")]
    private InputAction _rightAction;

    [SerializeField, Header("左方向のキーコン")]
    private InputAction _leftAction;

    [SerializeField, Header("上方向のキーコン")]
    private InputAction _upAction;

    [SerializeField, Header("下方向のキーコン")]
    private InputAction _downAction;

    [SerializeField, Header("選択のキーコン")]
    private InputAction _selectAction;

    [SerializeField, Header("")]
    RectTransform _backTransform;
   
    //現在開いているページ
    int _selectPage = 0;
    public int SelectPage { get { return _selectPage; } set { _selectPage = value; } }
    //現在選んでいるボタン
     int _selectIndex = 0;
    public int SelectIndex { get { return _selectIndex; } set { _selectIndex = value; } }
    [SerializeField]
    private SettingPage[] _pages;

    public int _soundVolume = 50;
    //長押し用のタイマー
    private float _timer = 0f;
    public bool _isPress = false;
    private void OnEnable()
    {
        EnableInput();
        OpenSetting();
        _pages[_selectPage]._buttons[_selectIndex].SetScale();
        //ゲーム停止
        Time.timeScale = 0f;
        //プレイヤーキャラの操作を不可に
        _player?.InputActionDisable();
    }

    //inputActionの停止とゲーム
    private void OnDisable()
    {
        
        _pages[_selectPage]._buttons[_selectIndex].ResetScale();
        _selectIndex = 0;
        //ゲーム再開
        Time.timeScale = 1f;
        //プレイヤー操作可能状態に
        _player.InputActionEnable();
    }
    public void EnableInput()
    {
        _upAction?.Enable();
        _downAction?.Enable();
        _rightAction?.Enable();
        _leftAction?.Enable();
        _selectAction?.Enable();
    }
    public void DisableInput()
    {
        _upAction?.Disable();
        _downAction?.Disable();
        _rightAction?.Disable();
        _leftAction?.Disable();
        _selectAction?.Disable();

    }
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        _player = FindAnyObjectByType<PlayerController_2D>();
        StartCoroutine(CloseSetting());
        gameObject.SetActive(false);
        int count = 0;
        foreach (var page in _pages)
        {
            page._pageNum = count;
            count++;
        }
        _pages[_selectPage]._buttons[_selectIndex].SetScale();
    }


    void Update()
    {
        if (_upAction.WasPressedThisFrame() && _selectIndex != 0)
        {
            SetSelect(_selectIndex - 1);
        }
        else if (_downAction.WasPressedThisFrame() && _selectIndex != _pages[_selectPage]._buttons.Length - 1)
        {
            SetSelect(_selectIndex + 1);
        }
        else if (_leftAction.WasPressedThisFrame() && _selectIndex > 0)
        {
            SetSelect(0);
        }
        else if (_rightAction.WasPressedThisFrame() && _selectIndex < _pages[_selectPage]._buttons.Length - 1)
        {
            SetSelect(_pages[_selectPage]._buttons.Length - 1);
        }
        else if (_selectAction.WasPressedThisFrame())
        {
            _isPress = true;
        }
        else if (_selectAction.WasReleasedThisFrame() && _isPress)
        {
            if (_timer < 0.5f)
            {
                _pages[_selectPage]._buttons[_selectIndex].SelectedButton();
            }
            else
            {
                _timer = 0;
            }
            _isPress = false;
        }
        if (_isPress) _timer += Time.unscaledDeltaTime;
    }
    /// <summary>
    /// ボタンの選択変える時に呼び出す関数
    /// </summary>
    /// <param name="_index">キーの位置</param>
    void SetSelect(int _index)
    {
        _pages[_selectPage]._buttons[_selectIndex].ResetScale();
        _pages[_selectPage]._buttons[_index].SetScale();
        _selectIndex = _index;
        //↓カーソルは過剰な気がするのでいったん削除
        //rectTransformをとるためにImageを取得する遠回り、遠すぎない？
        //_cursor.MoveCursor(_buttons[_index].GetComponent<Image>().rectTransform);
    }
    public IEnumerator CloseSetting()
    {
        DisableInput();
        _backTransform.DOScale(0,0.5f).SetUpdate(true);
        _backTransform.DOAnchorPos(new Vector2(0.5f,0.5f), 0.5f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(0.5f);
        gameObject.SetActive(false);
    }

    public void OpenSetting()
    {
        _backTransform.DOScale(1f,0.3f).SetUpdate(true);
        _backTransform.DOAnchorPos(Vector2.zero,0.3f).SetUpdate(true);
    }
    public void ChangePage(int nextPage)
    {
        if(nextPage >= _pages.Length)
        {
            Debug.LogError("Pageが範囲外です");
            return;
        }
        _pages[_selectPage]._buttons[_selectIndex].ResetScale();
        _pages[_selectPage].gameObject.SetActive(false);
        _selectPage = nextPage;
        _pages[_selectPage].gameObject.SetActive(true);
        SetSelect(0);
    }
    static public SettingManager GetInstance()
    {
        return instance;
    }

}
