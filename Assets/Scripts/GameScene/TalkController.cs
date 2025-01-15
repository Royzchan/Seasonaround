using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkController : MonoBehaviour
{
    CameraFollow2D _cameraFollow2D;

    [SerializeField] Transform _zoomObj;

    //プレイヤーとズームオブジェクトの中間地点にズーム
    Vector3 _zoomPos;
    GameObject _player;
    GameObject _TalkCanvas;
    bool _isPlayerHit = false;
    PlayerController_2D _playerController_2D;
    TalkObj _talkObj;
    PlayerController_2D _playerController2D;

    //文字送り
    public string[] names;//名前を格納
    public string[] sentences; // 文章を格納する

    [SerializeField] Text nameText;   // uiTextへの参照
    [SerializeField] Text sentenceText;   // uiTextへの参照

    [SerializeField]
    [Range(0.001f, 0.3f)]
    float intervalForCharDisplay = 0.05f;   // 1文字の表示にかける時間

    private int currentSentenceNum = 0; //現在表示している文章番号
    private string currentSentence = string.Empty;  // 現在の文字列
    private string currentName = string.Empty;  // 現在の文字列
    private float timeUntilDisplay = 0;     // 表示にかかる時間
    private float timeBeganDisplay = 1;         // 文字列の表示を開始した時間
    private int lastUpdateCharCount = -1;       // 表示中の文字数

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player").transform.root.gameObject;
        _playerController_2D = _player.GetComponent<PlayerController_2D>();
        _TalkCanvas = transform.GetChild(0).gameObject;
        _TalkCanvas.SetActive(false);
        _cameraFollow2D = Camera.main.gameObject.GetComponent<CameraFollow2D>();
        _talkObj = _zoomObj.GetComponent<TalkObj>();
        _playerController2D = _player.GetComponentInParent<PlayerController_2D>();
    }

    void Update()
    {
        if (!_isPlayerHit || !_playerController_2D.IsGround) return;

        // 文章の表示完了 / 未完了
        if (IsDisplayComplete())
        {
            //ボタンが押された
            if (Input.GetMouseButtonDown(0))
            {
                //最後の文章ではない
                if (currentSentenceNum < sentences.Length)
                {
                    SetNextSentence();
                }
                else
                {
                    //_playerController2D.InputActionEnable();
                    _cameraFollow2D.ZoomOut();
                    Destroy(this.gameObject);
                }
            }
        }
        else
        {
            //ボタンが押された
            if (Input.GetMouseButtonDown(0))
            {
                timeUntilDisplay = 0; //※1
            }
        }
        //表示される文字数を計算
        int displayCharCount = (int)(Mathf.Clamp01((Time.time - timeBeganDisplay) / timeUntilDisplay) * currentSentence.Length);
        //表示される文字数が表示している文字数と違う
        if (displayCharCount != lastUpdateCharCount)
        {
            sentenceText.text = currentSentence.Substring(0, displayCharCount);
            //表示している文字数の更新
            lastUpdateCharCount = displayCharCount;
        }

        nameText.text = currentName;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isPlayerHit)
        {
            _zoomPos = (_player.transform.position + _zoomObj.position) / 2;
            _cameraFollow2D.ZoomObjUpdate(2, _zoomPos);
            if (_playerController_2D.IsGround)
            {
                _TalkCanvas.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerController2D.InputActionDisable();
            _playerController2D.MoveStop(_zoomObj.position);
            _talkObj.Turn();
            _isPlayerHit = true;
            SetNextSentence();
        }
    }

    // 次の文章をセットする
    void SetNextSentence()
    {
        currentSentence = sentences[currentSentenceNum];
        currentName = names[currentSentenceNum];
        timeUntilDisplay = currentSentence.Length * intervalForCharDisplay;
        timeBeganDisplay = Time.time;
        currentSentenceNum++;
        lastUpdateCharCount = 0;
    }

    bool IsDisplayComplete()
    {
        return Time.time > timeBeganDisplay + timeUntilDisplay; //※2
    }
}
