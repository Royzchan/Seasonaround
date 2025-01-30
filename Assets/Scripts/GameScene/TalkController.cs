using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TalkController : MonoBehaviour
{
    [System.Serializable]
    public class AnimationSetting
    {
        [Header("関数を実行する行数")]
        public int _startLine = 0; // アニメーションを開始するテキストの行番号
        [Header("アニメーション変更関数")]
        public UnityEvent _animationEvent = new UnityEvent(); // アニメーションを実行するイベント
    }

    CameraFollow2D _cameraFollow2D;

    [SerializeField] Transform _talkObj; // 会話オブジェクトのTransform
    TalkObj _talkObjScript;

    //プレイヤーとズームオブジェクトの中間地点にズーム
    Vector3 _zoomPos; // カメラのズーム位置
    GameObject _player; // プレイヤーオブジェクト
    GameObject _TalkCanvas; // 会話用のCanvas

    PlayerController_2D _playerController_2D;

    //文字送り
    public string _name;// 会話キャラクターの名前
    public string[] _sentences;// 会話の文章を格納する配列

    [SerializeField] Text _nameText;   // 名前を表示するText
    [SerializeField] Text _sentenceText;   // 文章を表示するText

    [SerializeField]
    [Range(0.001f, 0.3f),Header("1文字の表示にかける時間")]
    float _intervalForCharDisplay = 0.05f;

    private int _currentSentenceNum = 0; //現在表示している文章番号
    private string _currentSentence = string.Empty;  // 現在表示する文章
    private float _timeUntilDisplay = 0;  // 次の文字が表示されるまでの時間
    private float _timeBeganDisplay = 1;  // 文字表示を開始した時間
    private int _lastUpdateCharCount = -1; // 最後に更新された文字数
    bool _isPlayerHit = false; // プレイヤーがトリガーに触れたかどうか
    bool _talkStart = true; // 会話開始フラグ

    [SerializeField] private List<AnimationSetting> _animationSettings = new List<AnimationSetting>(); // アニメーション設定のリスト

    [System.Serializable]
    public class TalkCameraSetting
    {
        [Header("カメラ移動する行数")]
        public int _line = 0; // カメラ設定を適用する文章の行番号
        [Header("会話シーンのズーム位置に移動")]
        public bool _back = false;
        [Header("ズーム位置")]
        public Transform _zoomPosition; //会話シーンの位置以外にズームするときに必要
    }

    [SerializeField] private List<TalkCameraSetting> _talkCameraSettings = new List<TalkCameraSetting>(); // 会話中のカメラ設定のリスト

    [SerializeField,Header("ズーム速度")] private float _zoomObjSpeed; // カメラのズーム速度
    [SerializeField,Header("ズーム量")] private float _zoomValue; // カメラのズーム量

    [SerializeField]
    GameObject _gameUI;

    [SerializeField, Header("会話終了時に実行したい関数")]
    UnityEvent _finishEvent = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player").transform.root.gameObject; // プレイヤーオブジェクトを取得
        _playerController_2D = _player.GetComponent<PlayerController_2D>(); // プレイヤーのコントローラースクリプトを取得
        _TalkCanvas = transform.GetChild(0).gameObject; // 会話用Canvasを取得
        _TalkCanvas.SetActive(false); // Canvasを非表示にする
        _cameraFollow2D = Camera.main.gameObject.GetComponent<CameraFollow2D>(); // メインカメラのカメラ追従スクリプトを取得
        _talkObjScript = _talkObj.GetComponent<TalkObj>();//会話オブジェクトのTalkObjスクリプトを取得
    }

    void Update()
    {
        // プレイヤーがトリガーに触れていないか、地面に接地していない場合は処理を中断
        if (!_isPlayerHit || !_playerController_2D.IsGround) return;

        // 文章の表示完了 / 未完了
        if (IsDisplayComplete())
        {
            //ボタンが押された
            if (Input.GetMouseButtonDown(0))
            {
                //最後の文章ではない
                if (_currentSentenceNum < _sentences.Length)
                {
                    SetNextSentence(); // 次の文章をセット
                }
                else
                {
                    _talkObjScript.IsIdle();//会話オブジェクトのアニメーションをアイドル状態にする
                    _cameraFollow2D.ZoomOut(); // カメラをズームアウトさせる
                    _gameUI.SetActive(true);
                    _finishEvent.Invoke();
                    Destroy(this.gameObject); // このオブジェクトを破棄する(会話シーンに入るのを1回にする)
                }
            }
        }
        else
        {
            //ボタンが押された
            if (Input.GetMouseButtonDown(0))
            {
                _timeUntilDisplay = 0; //※1 // すべての文字を即座に表示する
            }
        }
        //表示される文字数を計算
        int displayCharCount = (int)(Mathf.Clamp01((Time.time - _timeBeganDisplay) / _timeUntilDisplay) * _currentSentence.Length);
        //表示される文字数が表示している文字数と違う
        if (displayCharCount != _lastUpdateCharCount)
        {
            _sentenceText.text = _currentSentence.Substring(0, displayCharCount); // 文章を表示するテキストを更新
            //表示している文字数の更新
            _lastUpdateCharCount = displayCharCount; // 最後に表示した文字数を更新
        }

        _nameText.text = _name; // 名前を表示するテキストを更新
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isPlayerHit)
        {
            if (_playerController_2D.IsGround && _talkStart)
            {
                _talkStart = false; // 会話開始フラグを解除
                _TalkCanvas.SetActive(true); // 会話用Canvasを表示
                _zoomPos = (_player.transform.position + _talkObj.position) / 2; // プレイヤーと会話オブジェクトの中間地点を計算
                _cameraFollow2D.ZoomObj(_zoomValue,_zoomPos, _zoomObjSpeed); // カメラをズームインさせる
                SetNextSentence(); // 最初の文章をセット
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerController_2D.InputActionDisable(); // プレイヤーの入力を無効にする
            _playerController_2D.MoveStop(_talkObj.position); // プレイヤーの移動を停止する
            _isPlayerHit = true; // プレイヤーがトリガーに触れたことを記録
            _gameUI.SetActive(false);
        }
    }

    // 次の文章をセットする
    void SetNextSentence()
    {
        _currentSentence = _sentences[_currentSentenceNum]; // 現在の文章を設定
        _timeUntilDisplay = _currentSentence.Length * _intervalForCharDisplay; // 次の文字が表示されるまでの時間を計算
        _timeBeganDisplay = Time.time; // 文字表示を開始した時間を記録

        //アニメーションの設定をチェック
        foreach (var setting in _animationSettings)
        {
            if (_currentSentenceNum == setting._startLine)
            {
                setting._animationEvent.Invoke(); // アニメーションイベントを実行
                break; // 一致する設定が見つかったら、それ以上ループする必要はない
            }
        }

        //カメラの設定をチェック
        foreach (var setting in _talkCameraSettings)
        {
            if (_currentSentenceNum == setting._line)
            {
                if (setting._back)
                {
                    _cameraFollow2D.ZoomObj(_zoomValue,_zoomPos, _zoomObjSpeed); // カメラをズームインさせる
                }
                else
                {
                    _cameraFollow2D.ZoomObj(_zoomValue,setting._zoomPosition.position, _zoomObjSpeed); // カメラをズームインさせる
                    break; // 一致する設定が見つかったら、それ以上ループする必要はない
                }
            }
        }

        _currentSentenceNum++; // 現在の文章のインデックスを更新
        _lastUpdateCharCount = 0; // 最後に表示した文字数をリセット
    }

    // 全ての文字が表示されたかをチェックする
    bool IsDisplayComplete()
    {
        return Time.time > _timeBeganDisplay + _timeUntilDisplay; // 文字表示を開始した時間と表示にかかる時間を足した値が、現在の時間より大きければ表示完了
    }
}