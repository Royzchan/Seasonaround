using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkController : MonoBehaviour
{
    CameraFollow2D _cameraFollow2D;

    [SerializeField] Transform _zoomObj;

    //�v���C���[�ƃY�[���I�u�W�F�N�g�̒��Ԓn�_�ɃY�[��
    Vector3 _zoomPos;
    GameObject _player;
    GameObject _TalkCanvas;
    bool _isPlayerHit = false;
    PlayerController_2D _playerController_2D;
    TalkObj _talkObj;
    PlayerController_2D _playerController2D;

    //��������
    public string[] names;//���O���i�[
    public string[] sentences; // ���͂��i�[����

    [SerializeField] Text nameText;   // uiText�ւ̎Q��
    [SerializeField] Text sentenceText;   // uiText�ւ̎Q��

    [SerializeField]
    [Range(0.001f, 0.3f)]
    float intervalForCharDisplay = 0.05f;   // 1�����̕\���ɂ����鎞��

    private int currentSentenceNum = 0; //���ݕ\�����Ă��镶�͔ԍ�
    private string currentSentence = string.Empty;  // ���݂̕�����
    private string currentName = string.Empty;  // ���݂̕�����
    private float timeUntilDisplay = 0;     // �\���ɂ����鎞��
    private float timeBeganDisplay = 1;         // ������̕\�����J�n��������
    private int lastUpdateCharCount = -1;       // �\�����̕�����

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

        // ���͂̕\������ / ������
        if (IsDisplayComplete())
        {
            //�{�^���������ꂽ
            if (Input.GetMouseButtonDown(0))
            {
                //�Ō�̕��͂ł͂Ȃ�
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
            //�{�^���������ꂽ
            if (Input.GetMouseButtonDown(0))
            {
                timeUntilDisplay = 0; //��1
            }
        }
        //�\������镶�������v�Z
        int displayCharCount = (int)(Mathf.Clamp01((Time.time - timeBeganDisplay) / timeUntilDisplay) * currentSentence.Length);
        //�\������镶�������\�����Ă��镶�����ƈႤ
        if (displayCharCount != lastUpdateCharCount)
        {
            sentenceText.text = currentSentence.Substring(0, displayCharCount);
            //�\�����Ă��镶�����̍X�V
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

    // ���̕��͂��Z�b�g����
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
        return Time.time > timeBeganDisplay + timeUntilDisplay; //��2
    }
}
