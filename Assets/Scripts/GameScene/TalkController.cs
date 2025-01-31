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
        [Header("�֐������s����s��")]
        public int _startLine = 0; // �A�j���[�V�������J�n����e�L�X�g�̍s�ԍ�
        [Header("�A�j���[�V�����ύX�֐�")]
        public UnityEvent _animationEvent = new UnityEvent(); // �A�j���[�V���������s����C�x���g
    }

    CameraFollow2D _cameraFollow2D;

    [SerializeField] Transform _talkObj; // ��b�I�u�W�F�N�g��Transform
    TalkObj _talkObjScript;

    //�v���C���[�ƃY�[���I�u�W�F�N�g�̒��Ԓn�_�ɃY�[��
    Vector3 _zoomPos; // �J�����̃Y�[���ʒu
    GameObject _player; // �v���C���[�I�u�W�F�N�g
    GameObject _TalkCanvas; // ��b�p��Canvas

    PlayerController_2D _playerController_2D;

    //��������
    public string _name;// ��b�L�����N�^�[�̖��O
    public string[] _sentences;// ��b�̕��͂��i�[����z��

    [SerializeField] Text _nameText;   // ���O��\������Text
    [SerializeField] Text _sentenceText;   // ���͂�\������Text

    [SerializeField]
    [Range(0.001f, 0.3f),Header("1�����̕\���ɂ����鎞��")]
    float _intervalForCharDisplay = 0.05f;

    private int _currentSentenceNum = 0; //���ݕ\�����Ă��镶�͔ԍ�
    private string _currentSentence = string.Empty;  // ���ݕ\�����镶��
    private float _timeUntilDisplay = 0;  // ���̕������\�������܂ł̎���
    private float _timeBeganDisplay = 1;  // �����\�����J�n��������
    private int _lastUpdateCharCount = -1; // �Ō�ɍX�V���ꂽ������
    bool _isPlayerHit = false; // �v���C���[���g���K�[�ɐG�ꂽ���ǂ���
    bool _talkStart = true; // ��b�J�n�t���O

    [SerializeField] private List<AnimationSetting> _animationSettings = new List<AnimationSetting>(); // �A�j���[�V�����ݒ�̃��X�g

    [System.Serializable]
    public class TalkCameraSetting
    {
        [Header("�J�����ړ�����s��")]
        public int _line = 0; // �J�����ݒ��K�p���镶�͂̍s�ԍ�
        [Header("��b�V�[���̃Y�[���ʒu�Ɉړ�")]
        public bool _back = false;
        [Header("�Y�[���ʒu")]
        public Transform _zoomPosition; //��b�V�[���̈ʒu�ȊO�ɃY�[������Ƃ��ɕK�v
    }

    [SerializeField] private List<TalkCameraSetting> _talkCameraSettings = new List<TalkCameraSetting>(); // ��b���̃J�����ݒ�̃��X�g

    [SerializeField,Header("�Y�[�����x")] private float _zoomObjSpeed; // �J�����̃Y�[�����x
    [SerializeField,Header("�Y�[����")] private float _zoomValue; // �J�����̃Y�[����

    [SerializeField]
    GameObject _gameUI;

    [SerializeField, Header("��b�I�����Ɏ��s�������֐�")]
    UnityEvent _finishEvent = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player").transform.root.gameObject; // �v���C���[�I�u�W�F�N�g���擾
        _playerController_2D = _player.GetComponent<PlayerController_2D>(); // �v���C���[�̃R���g���[���[�X�N���v�g���擾
        _TalkCanvas = transform.GetChild(0).gameObject; // ��b�pCanvas���擾
        _TalkCanvas.SetActive(false); // Canvas���\���ɂ���
        _cameraFollow2D = Camera.main.gameObject.GetComponent<CameraFollow2D>(); // ���C���J�����̃J�����Ǐ]�X�N���v�g���擾
        _talkObjScript = _talkObj.GetComponent<TalkObj>();//��b�I�u�W�F�N�g��TalkObj�X�N���v�g���擾
    }

    void Update()
    {
        // �v���C���[���g���K�[�ɐG��Ă��Ȃ����A�n�ʂɐڒn���Ă��Ȃ��ꍇ�͏����𒆒f
        if (!_isPlayerHit || !_playerController_2D.IsGround) return;

        // ���͂̕\������ / ������
        if (IsDisplayComplete())
        {
            //�{�^���������ꂽ
            if (Input.GetMouseButtonDown(0))
            {
                //�Ō�̕��͂ł͂Ȃ�
                if (_currentSentenceNum < _sentences.Length)
                {
                    SetNextSentence(); // ���̕��͂��Z�b�g
                }
                else
                {
                    _talkObjScript.IsIdle();//��b�I�u�W�F�N�g�̃A�j���[�V�������A�C�h����Ԃɂ���
                    _cameraFollow2D.ZoomOut(); // �J�������Y�[���A�E�g������
                    _gameUI.SetActive(true);
                    _finishEvent.Invoke();
                    Destroy(this.gameObject); // ���̃I�u�W�F�N�g��j������(��b�V�[���ɓ���̂�1��ɂ���)
                }
            }
        }
        else
        {
            //�{�^���������ꂽ
            if (Input.GetMouseButtonDown(0))
            {
                _timeUntilDisplay = 0; //��1 // ���ׂĂ̕����𑦍��ɕ\������
            }
        }
        //�\������镶�������v�Z
        int displayCharCount = (int)(Mathf.Clamp01((Time.time - _timeBeganDisplay) / _timeUntilDisplay) * _currentSentence.Length);
        //�\������镶�������\�����Ă��镶�����ƈႤ
        if (displayCharCount != _lastUpdateCharCount)
        {
            _sentenceText.text = _currentSentence.Substring(0, displayCharCount); // ���͂�\������e�L�X�g���X�V
            //�\�����Ă��镶�����̍X�V
            _lastUpdateCharCount = displayCharCount; // �Ō�ɕ\���������������X�V
        }

        _nameText.text = _name; // ���O��\������e�L�X�g���X�V
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isPlayerHit)
        {
            if (_playerController_2D.IsGround && _talkStart)
            {
                _talkStart = false; // ��b�J�n�t���O������
                _TalkCanvas.SetActive(true); // ��b�pCanvas��\��
                _zoomPos = (_player.transform.position + _talkObj.position) / 2; // �v���C���[�Ɖ�b�I�u�W�F�N�g�̒��Ԓn�_���v�Z
                _cameraFollow2D.ZoomObj(_zoomValue,_zoomPos, _zoomObjSpeed); // �J�������Y�[���C��������
                SetNextSentence(); // �ŏ��̕��͂��Z�b�g
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerController_2D.InputActionDisable(); // �v���C���[�̓��͂𖳌��ɂ���
            _playerController_2D.MoveStop(_talkObj.position); // �v���C���[�̈ړ����~����
            _isPlayerHit = true; // �v���C���[���g���K�[�ɐG�ꂽ���Ƃ��L�^
            _gameUI.SetActive(false);
        }
    }

    // ���̕��͂��Z�b�g����
    void SetNextSentence()
    {
        _currentSentence = _sentences[_currentSentenceNum]; // ���݂̕��͂�ݒ�
        _timeUntilDisplay = _currentSentence.Length * _intervalForCharDisplay; // ���̕������\�������܂ł̎��Ԃ��v�Z
        _timeBeganDisplay = Time.time; // �����\�����J�n�������Ԃ��L�^

        //�A�j���[�V�����̐ݒ���`�F�b�N
        foreach (var setting in _animationSettings)
        {
            if (_currentSentenceNum == setting._startLine)
            {
                setting._animationEvent.Invoke(); // �A�j���[�V�����C�x���g�����s
                break; // ��v����ݒ肪����������A����ȏニ�[�v����K�v�͂Ȃ�
            }
        }

        //�J�����̐ݒ���`�F�b�N
        foreach (var setting in _talkCameraSettings)
        {
            if (_currentSentenceNum == setting._line)
            {
                if (setting._back)
                {
                    _cameraFollow2D.ZoomObj(_zoomValue,_zoomPos, _zoomObjSpeed); // �J�������Y�[���C��������
                }
                else
                {
                    _cameraFollow2D.ZoomObj(_zoomValue,setting._zoomPosition.position, _zoomObjSpeed); // �J�������Y�[���C��������
                    break; // ��v����ݒ肪����������A����ȏニ�[�v����K�v�͂Ȃ�
                }
            }
        }

        _currentSentenceNum++; // ���݂̕��͂̃C���f�b�N�X���X�V
        _lastUpdateCharCount = 0; // �Ō�ɕ\�����������������Z�b�g
    }

    // �S�Ă̕������\�����ꂽ�����`�F�b�N����
    bool IsDisplayComplete()
    {
        return Time.time > _timeBeganDisplay + _timeUntilDisplay; // �����\�����J�n�������Ԃƕ\���ɂ����鎞�Ԃ𑫂����l���A���݂̎��Ԃ��傫����Ε\������
    }
}