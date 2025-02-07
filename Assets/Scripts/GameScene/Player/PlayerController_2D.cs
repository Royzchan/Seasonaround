using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;


public enum Animal
{
    Normal,//�ʏ���
    Colobus,//�S����
    Gecko,//�g�J�Q
    Herring,//��
    Muskrat,//���X
    Pudu,//��
    Sparrow,//��
    Squid,//�C�J
    Taipan//��
}

public class PlayerController_2D : MonoBehaviour, IDamageable
{
    private Rigidbody _rb;

    //���݂Ȃ�̓������ǂ���
    private Animal _nowAnimal = Animal.Normal;
    public Animal NowAnimal {  get { return _nowAnimal; } } 
    [SerializeField, Header("�v���C���[�̈ړ����x")]
    private float _speed = 5.0f;

    [SerializeField, Header("�v���C���[�̍ō����x")]
    private float _maxSpeed = 5.0f;

    [SerializeField, Header("�v���C���[�̉j�����x")]
    private float _swimSpeed = 5.0f;

    [SerializeField, Header("�v���C���[�̃W�����v��")]
    private float _jumpPower = 5.0f;

    [SerializeField, Header("�X�^�~�i�̍ő�l")]
    private float _maxStamina = 100;
    [SerializeField, Header("�X�^�~�i�̎g�p�l")]
    private float _useFrameStamina = 20f;
    [SerializeField, Header("�X�^�~�i�񕜗�")]
    private float _healStamina = 5f;
    private float _nowStamina;

    [SerializeField, Header("��ԃX�s�[�h")]
    private float _flySpeed = 5.0f;

    //���͂̒l��ۑ�����ϐ�
    private float _inputValueX = 0;

    //�W�����v�̉�
    private int _jumpNum = 0;

    [SerializeField, Header("�W�����v�ł����")]
    private int _canJumpNum = 1;

    //�W�����v�����ǂ����̔���
    private bool _jumpNow = false;

    //���ł��邩�ǂ���
    private bool _isFly = false;

    //��ׂ邩�ǂ���
    private bool _canFly = true;

    public bool ISFly { get { return _isFly; } }

    //�G�ꂽ�G��|���邩�ǂ���
    private bool _canDefeatEnemy = false;

    [SerializeField, Header("���͌���(�|�����)"), Range(0.9f, 0.99f)]
    private float _windLatePower;
    private Vector3 _windPower;

    [SerializeField, Header("�W���������x(���m�ɂ͂�����ƈႤ)")]
    private float _defaultDrag;
    [SerializeField, Header("����t����Ԃ̒�R")]
    private float _wallDrag = 5f;
    [SerializeField, Header("�������������x")]
    private float _waterDrag;
    //�����������Ƃ̂���(�ϐg�\��)����
    private List<Animal> _hitAnimals = new();

    [SerializeField, Header("���̃X�e�[�W�ŕϐg�\�ȓ���(�L�[���蓖��   0:��,1:��,2:��,3:��)")]
    private Animal[] _stageAnimals = new Animal[4];
    //�ϐg�L�[��������Ă���
    private bool _canChangeAnimal = false;

    private Vector3 _preGroundPos = new Vector3();
    int _selectAnimalNum = -1;
    Animator _animator;

    public Animator Animator { get { return _animator; } }

    [SerializeField]
    SelectUIScript _selectUI;
    [SerializeField]
    StaminaUIScript _staminaUI;
    [SerializeField, Header("�����̃��f��")]
    private GameObject[] _animalModels;
    [SerializeField, Header("�ϐg�p�̉�")]
    private ParticleSystem _smokeParticle;
    bool _isGround = false;
    public bool IsGround { get { return _isGround; } }

    public bool IsSwimming { get => _isSwimming; set => _isSwimming = value; }

    bool _isSwimming = false;
    bool _isGrap = false;
    bool _isMoveGrap = false;
    bool _usedStamina = false;
    GrapGimmick _grapData = null;
    HingeJoint _joint = null;
    [Header("�`�C���v�b�g�֌W�`")]
    [SerializeField, Header("���ړ��̃L�[�R��")]
    private InputAction _moveXAction;

    [SerializeField, Header("�����͂̃L�[�R��")]
    private InputAction _underAction;

    [SerializeField, Header("�W�����v�̃L�[�R��")]
    private InputAction _jumpAction;

    [SerializeField, Header("�ŗL�\�͂̃L�[�R��")]
    private InputAction _animalAbilityAction;

    [SerializeField, Header("�ϐg�p�̃L�[�R��")]
    private InputAction _changeAction;

    [SerializeField, Header("�������͂̃L�[�R��")]
    private InputAction _cursorUpAction;
    [SerializeField]
    private InputAction _cursorRightAction;
    [SerializeField]
    private InputAction _cursorDownAction;
    [SerializeField]
    private InputAction _cursorLeftAction;

    [SerializeField, Header("�v���C���[��HP")]
    private int _hp;

    [SerializeField, Header("HP�̍ő�l")]
    int _maxHp;

    [SerializeField, Header("�h�炷�J����")]
    public GameObject _movecamera;
    private float moveCameraX;
    private float moveCameraY;
    [SerializeField]
    private Vector3 _cameraDistance = new Vector3(0f, 0f, 0f);

    [SerializeField, Header("���U���p�̃^�C�����C��")]
    private TimelineAsset _underAttackTimeline;

    [SerializeField, Header("���U���p�̃^�C�����C��")]
    private TimelineAsset _sideAttackTimeline;
    // �Q�[���I�[�o�[�X�N���v�g���Q��
    public GameOverScript gameOverScript;

    private float s;
    private float v;


    // �L����
    private void OnEnable()
    {
        InputActionEnable();
    }

    // ������
    private void OnDisable()
    {
        InputActionDisable();
    }

    public void InputActionEnable()
    {
        // InputAction��L����
        _moveXAction?.Enable();
        _jumpAction?.Enable();
        _animalAbilityAction?.Enable();
        _changeAction?.Enable();
        _cursorUpAction?.Enable();
        _cursorRightAction?.Enable();
        _cursorDownAction?.Enable();
        _cursorLeftAction?.Enable();
        _underAction?.Enable();
    }

    public void InputActionDisable()
    {
        // ���g�������������^�C�~���O�Ȃǂ�
        _moveXAction?.Disable();
        _jumpAction?.Disable();
        _animalAbilityAction?.Disable();
        _changeAction?.Disable();
        _cursorUpAction?.Disable();
        _cursorRightAction?.Disable();
        _cursorDownAction?.Disable();
        _cursorLeftAction?.Disable();
        _underAction?.Disable();
    }

    public void MoveStop(Vector3 zoomObjPos)
    {
        //��b�ɂȂ������̒�~�p
        _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, 0.6f);
        LookZoomObj(zoomObjPos);
    }

    void LookZoomObj(Vector3 zoomObjPos)
    {
        if (zoomObjPos.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (zoomObjPos.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public int Hp
    {
        get { return _hp; }
        set
        {
            _hp = Mathf.Clamp(value, 0, _maxHp);

            if (_hp <= 0)
            {
                Death();
            }
        }
    }

    void Start()
    {
        RestartManager rm = FindAnyObjectByType<RestartManager>();
        if (rm != null)
        {
            transform.position = rm.GetRestartPosition();
        }

        Camera.main.transform.position = transform.position + _cameraDistance;
        _movecamera = Camera.main.gameObject;
        _preGroundPos = transform.position;
        //���W�b�h�{�f�B���擾
        _rb = GetComponent<Rigidbody>();
        //���f����Active�Ȃ����Animator���擾
        foreach (var body in _animalModels)
        {

            if (body != null && body.activeSelf)
            {
                _animator = body.GetComponent<Animator>();
                break;
            }
        }
        _staminaUI = FindAnyObjectByType<StaminaUIScript>();
        //�X�^�~�i���ő�l���Z�b�g
        _nowStamina = _maxStamina;

        Hp = _maxHp;

        moveCameraX = 1;
        moveCameraY = 1;

    }

    void Update()
    {

        //X���̃L�[���͂�ۑ�
        _inputValueX = _moveXAction.ReadValue<float>();
        if (_inputValueX != 0)
        {
            //�ǒ���t�����͐U��ނ��Ȃ��悤��
            if (_rb.useGravity)
            {
                transform.localScale = new Vector3(_inputValueX * Mathf.Abs(transform.localScale.x),
                    transform.localScale.y,
                    transform.localScale.z);
            }

            if (!_isGrap)
            {
                _animator.Play("Walk");
            }

        }

        //�W�����v�̃{�^���������ꂽ��
        if (_jumpAction.WasPressedThisFrame())
        {
            if (!_isSwimming)
            {
                //�W�����v�����񐔂��W�����v�\�񐔂��Ⴉ������
                if (_jumpNum < _canJumpNum)
                {
                    //��ɗ͂�������
                    _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
                    _rb.AddForce(transform.rotation * new Vector3(0f, _jumpPower, 0f), ForceMode.Impulse);
                    //�W�����v�����񐔂��v���X
                    _jumpNum++;
                    //�W�����v���̔����true��
                    _jumpNow = true;
                }
            }
            else
            {
                //�����Ȃ炻�̂܂�
                _rb.AddForce(new Vector3(0f, _jumpPower, 0f), ForceMode.Impulse);
            }
        }

        //�����̌ŗL�\�͂̃{�^����������Ă�����
        if (_animalAbilityAction.IsPressed())
        {
            //�����̌ŗL�\�͂��g��
            AnimalAction();
        }
        if (_animalAbilityAction.WasReleasedThisFrame())
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0f));
            _rb.useGravity = true;
            _isGrap = false;
            Destroy(_joint);
            _joint = null;
        }
        //�W�����v����������
        //if (_jumpNow)
        //{
            
        //}

        // Ray���v���C���[�̉����ɔ�΂�
        Ray ray = new Ray(_animalModels[(int)_nowAnimal].transform.position, -transform.up);
        //Debug.DrawRay(transform.position, -transform.up * 0.01f, Color.yellow,0.1f);
        RaycastHit hit;

        // ���C�L���X�g�����s���āA�����ɓ������������m�F
        if (Physics.Raycast(ray, out hit, 0.2f))
        {

            //���C���G�ɓ������Ă�����
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                //�G��|����悤��
                _canDefeatEnemy = true;
            }
            //�Ⴄ�����ɓ������Ă�����
            else if (hit.collider.CompareTag("Ground"))
            {
                //�G��|���锻���false��
                _canDefeatEnemy = false;
                JumpReset();
            }
            else
            {
                //�G��|���锻���false��
                _canDefeatEnemy = false;
            }
        }
        //���ɂ��������Ă��Ȃ�������
        else
        {
            //�G��|���锻���false��
            _canDefeatEnemy = false;
        }
        //Debug.DrawRay(ray.origin, ray.direction, UnityEngine.Color.red, 5.0f);

        if (_canChangeAnimal)
        {
            //��
            if (_cursorUpAction.WasPressedThisFrame())
            {
                _selectAnimalNum = 0;
                _selectUI.ScaleMove(_selectAnimalNum);
            }
            //��
            else if (_cursorRightAction.WasPressedThisFrame())
            {
                _selectAnimalNum = 1;
                _selectUI.ScaleMove(_selectAnimalNum);
            }
            //��
            else if (_cursorDownAction.WasPressedThisFrame())
            {
                _selectAnimalNum = 2;
                _selectUI.ScaleMove(_selectAnimalNum);
            }
            //��
            else if (_cursorLeftAction.WasPressedThisFrame())
            {
                _selectAnimalNum = 3;
                _selectUI.ScaleMove(_selectAnimalNum);
            }
            DebugAddHitAnimal(_selectAnimalNum);
        }
        //�ϐg�{�^�����������Ƃ�(�����ł͕ϐg�s��)
        if (_changeAction.IsPressed())
        {

            if (!_isSwimming)
            {
                _canChangeAnimal = true;
                _selectUI.gameObject.SetActive(true);
            }
        }
        //�������Ƃ�
        else if (_canChangeAnimal)
        {


            //�����l�łȂ����
            if (_selectAnimalNum != -1)
            {
                foreach (var animal in _hitAnimals)
                {
                    if (animal == _stageAnimals[_selectAnimalNum])
                    {
                        InputChangeAnimal(_stageAnimals[_selectAnimalNum]);

                        break;
                    }
                }
            }
            _selectUI.ResetScale();
            _selectAnimalNum = -1;
            _selectUI.gameObject.SetActive(false);
            _canChangeAnimal = false;
        }

        if (_usedStamina)
        {
            if (_isGround)
            {
                _usedStamina = false;
            }

        }
        else
        {
            //��
            _nowStamina += _healStamina * Time.deltaTime;
        }
        _nowStamina = Mathf.Clamp(_nowStamina, 0, _maxStamina);
        _staminaUI.GaugeUpdate(_nowStamina, _maxStamina);
    }

    private void FixedUpdate()
    {
        if (!_isSwimming && _isGround)
        {
            _preGroundPos = transform.position;
        }
        float speed = _speed;
        if (_isSwimming)
        {
            speed = _swimSpeed;
        }
        //���x�����ړ��̒l��
        if (Mathf.Sign(_rb.velocity.x) == Mathf.Sign(_inputValueX))
        {
            if (Mathf.Abs(_rb.velocity.x) < _maxSpeed)
            {
                _rb.velocity += transform.rotation * new Vector3(_inputValueX * speed, 0f, 0f);
            }
        }
        else
        {
            _rb.velocity += transform.rotation * new Vector3(_inputValueX * speed, 0f, 0f);
        }
        //if(!_isGrap)
        //{
        //    _rb.velocity = new Vector3(Mathf.Clamp(_rb.velocity.x, -_maxSpeed, _maxSpeed), _rb.velocity.y) + _windPower;
        //}

        WindPowDown();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //�G��|���锻�肪true��������
            if (_canDefeatEnemy)
            {
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            if (_nowAnimal == Animal.Gecko)
            {
                _isSwimming = true;
                _rb.drag = _waterDrag;
            }
        }
        if (other.CompareTag("Death"))
        {
            Death();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //�c�^�ɓ������Ă�Ԃ��̏����擾
        if (other.CompareTag("Grap") && _grapData == null)
        {
            _grapData = other.GetComponent<GrapGimmick>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            _isSwimming = false;
            //��R�����Ƃɖ߂�
            _rb.drag = _defaultDrag;
            //�ǉ��ŃW�����v�ł��Ȃ��悤��
            _jumpNum = _canJumpNum;
        }
        //�ړ��A�j���[�V�������̓X�g�b�v
        else if (other.CompareTag("Grap") && !_isMoveGrap)
        {
            _grapData = null;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {

            _isGround = false;
        }
    }

    void DebugAddHitAnimal(int _selectAnimal)
    {
#if UNITY_EDITOR
        //�����l�Ȃ珈�����s��Ȃ�
        if (_selectAnimal == -1) return;
        //hitAnimals�ɓ����Ă��Ȃ�&controll��������Ă���
        if (Input.GetKey(KeyCode.LeftControl) && !_hitAnimals.Contains(_stageAnimals[_selectAnimal]))
        {
            _hitAnimals.Add(_stageAnimals[_selectAnimal]);
            _selectUI.SetHitAnimal(_selectAnimal);
        }
#endif
    }

    private void JumpReset()
    {
        _jumpNum = 0;
        _jumpNow = false;
        _isFly = false;
        _isGround = true;
    }
    //�_���[�W
    public void HitDamage()
    {
        //HP�����炷
        _hp--;
    }

    public void SetWindPow(Vector3 addPower)
    {
        _windPower = addPower;
    }

    private void WindPowDown()
    {
        //_windPower�����l�܂ŉ��������Ȃ�0�ɂ���return
        if (Mathf.Abs(_windPower.x) < 0.1f)
        {
            _windPower.x = 0;
        }
        if (Mathf.Abs(_windPower.y) < 0.1f)
        {
            _windPower.y = 0;
        }
        if (_windPower == Vector3.zero) return;

        //����
        _windPower *= _windLatePower;

        //���E�C�̂�����̂��͂����A���ɂ͂��ꂪ���E������
        if (_windPower.y != 0)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _windPower.y);
        }

        //�������قȂ�Ƃ�
        if (Mathf.Sign(_inputValueX) != Mathf.Sign(_windPower.x) && _inputValueX != 0)
        {
            //����ɒǉ��Ō���
            _windPower.x *= _windLatePower;
            //_speed��菬�����͂ɂȂ��0��
            if (Mathf.Abs(_windPower.x) < _speed)
            {
                _windPower.x = 0;
            }
        }
    }

    IEnumerator MoveGrapPosition()
    {
        //animation���͒�~
        _isMoveGrap = true;
        _rb.isKinematic = true;
        //�󒆂ň�U�Î~(���o�A����Ȃ��C�����Ȃ��ł��Ȃ�)
        yield return new WaitForSeconds(0.15f);
        Vector3 prePos = transform.position;
        Vector3 grapPos = _grapData.transform.position + _grapData.transform.rotation * _grapData._grapPos;
        float timer = 0f;
        //������0.3f,���͎��Ԃ�0.8f��������܂ŌJ��Ԃ�
        while (true)
        {
            timer += Time.deltaTime;
            //deltaTime�̕��ҋ@(���̕����E�E�E�Ȃ񂩕ρE�E�E)
            yield return null;
            grapPos = _grapData.transform.position + _grapData.transform.rotation * _grapData._grapPos;
            //�ړ�
            transform.position = Vector3.Lerp(prePos, grapPos, timer / 0.8f);

            if (timer > 0.8f || Vector3.Distance(transform.position, _grapData.transform.position + _grapData.transform.rotation * _grapData._grapPos) < 0.3f)
            {
                //�ʒu��grapPos�ɌŒ�
                transform.position = _grapData.transform.position + _grapData.transform.rotation * _grapData._grapPos;
                transform.rotation = _grapData.transform.rotation;
                _rb.isKinematic = false;
                break;
            }
        }
        //animation�I��
        _isMoveGrap = false;
        CreateGrapJoint();
    }

    void CreateGrapJoint()
    {
        if (_grapData != null)
        {
            _joint = gameObject.AddComponent<HingeJoint>();
            _joint.axis = new Vector3(0, 0, 1f);
            _joint.connectedBody = _grapData.gameObject.GetComponent<Rigidbody>();
        }

    }
    //�����̌ŗL�A�N�V����
    private void AnimalAction()
    {
        switch (_nowAnimal)
        {
            //�X���C��
            case Animal.Normal:
                //�O���b�v�\����joint����
                if (_joint == null && !_isGrap && _grapData != null)
                {
                    //transform.position = _grapData.transform.position + _grapData.transform.rotation * _grapData._grapPos;
                    _isGrap = true;
                    StartCoroutine(MoveGrapPosition());
                }
                else if (_joint != null && _grapData != null)
                {
                    //���[�e�[�V�����̌Œ�
                    transform.rotation = _grapData.transform.rotation;
                }
                break;
            //�S����
            case Animal.Colobus:
                //_animator.Play("Attack");
                var playable = _animalModels[(int)Animal.Colobus].GetComponent<PlayableDirector>();
                if (_underAction.IsPressed())
                {
                    playable.playableAsset = _underAttackTimeline;
                }
                else
                {
                    playable.playableAsset = _sideAttackTimeline;
                }
                playable.Play();
                break;

            //�g�J�Q
            case Animal.Gecko:
                float rayLength = 0.7f;
                //�E�������C
                Ray ray = new Ray(transform.position, Vector3.right);
                Debug.DrawRay(transform.position, Vector3.right * rayLength, Color.red, 2f);
                RaycastHit hit;
                if (_nowStamina <= 0)
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0f));
                    _rb.useGravity = true;
                }
                // ���C�L���X�g�����s���āA�����ɓ������������m�F
                else if (Physics.Raycast(ray, out hit, rayLength, 1 << 8))
                {
                    //Debug.Log("���������Ă��");
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90f));
                    _rb.useGravity = false;
                    _rb.velocity *= _wallDrag * Time.deltaTime;
                    _nowStamina -= _useFrameStamina * Time.deltaTime;
                    _usedStamina = true;
                }
                else
                {
                    //���������C
                    ray = new Ray(transform.position, Vector3.left);
                    // ���C�L���X�g�����s���āA�����ɓ������������m�F
                    if (Physics.Raycast(ray, out hit, rayLength, 1 << 8))
                    {
                        //Debug.Log("���������Ă��");
                        transform.rotation = Quaternion.Euler(new Vector3(180, 180f, 90f));
                        _rb.useGravity = false;
                        _rb.velocity *= _wallDrag * Time.deltaTime;
                        _nowStamina -= _useFrameStamina * Time.deltaTime;
                        _usedStamina = true;
                    }
                    else
                    {
                        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0f));
                        _rb.useGravity = true;
                    }
                }

                break;

            //��
            case Animal.Sparrow:
                //_rb.velocity = new Vector3((_speed * _inputValueX), _flySpeed, 0f);
                _rb.AddForce(new Vector3(0, _flySpeed, 0));
                _isFly = true;
                break;
        }
    }

    //�����ɐG�ꂽ���ɕԐM���鏈��
    public void ChangeAnimal(Animal animal)
    {
        //_animalModels[(int)_nowAnimal].SetActive(false);
        _nowAnimal = animal;
        //_animalModels[(int)_nowAnimal].SetActive(true);
        //���f���̔z����m�F���Ă���
        for (int i = 0; i < _animalModels.Length; i++)
        {
            //�����ԐM���铮���̃��f���ɂȂ�����
            if (i == (int)animal)
            {
                //���̓����̃��f�����Z�b�g
                _animalModels[i].SetActive(true);
                //���̓����̃A�j���[�^�[���擾
                _animator = _animalModels[i].GetComponent<Animator>();
            }
            //����ȊO�̓����������ꍇ
            else if (_animalModels[i] != null)
            {
                //�����̃��f��������
                _animalModels[i].SetActive(false);
            }

        }
        _smokeParticle.Play();
        _hitAnimals.Add(animal);
        for (int i = 0; i < _stageAnimals.Length; ++i)
        {
            if (_stageAnimals[i] == animal)
            {
                _selectUI.SetHitAnimal(i);
                break;
            }
        }
        SetState((int)animal);
    }

    private void SetState(int dataNum)
    {
        _maxSpeed = AnimalData.animalDatas[dataNum].Speed;
        _jumpPower = AnimalData.animalDatas[dataNum].JumpPower;
    }

    public void InputChangeAnimal(Animal animal)
    {

        //�O�̃I�u�W�F�N�g��false
        _animalModels[(int)_nowAnimal].SetActive(false);
        //�ύX
        _nowAnimal = animal;
        _smokeParticle.Play();
        _animalModels[(int)_nowAnimal].SetActive(true);
        _animator = _animalModels[(int)_nowAnimal].GetComponent<Animator>();
        SetState((int)animal);
    }


    public void Damage(int value)
    {
        Hp -= value;
        StartCoroutine(CameraShake());
    }

    public void Death()
    {
        // GameOverScript���Ăяo���ăQ�[���I�[�o�[���������s
        if (gameOverScript != null)
        {
            gameOverScript.SendMessage("GameOver");
        }
        else
        {
            Debug.LogError("GameOverScript���ݒ肳��Ă��܂���I");
        }

        _movecamera = null;

        //// �v���C���[�I�u�W�F�N�g��j��
        //Destroy(gameObject);
    }

    IEnumerator CameraShake()
    {
        for (int i = 0; i < 10; i++)
        {
            _movecamera.transform.Translate(moveCameraX, moveCameraY, 0);
            moveCameraX *= -1;
            moveCameraY *= -1;
            yield return new WaitForSeconds(0.01f);
        }
    }
}

public interface IDamageable
{
    public void Damage(int value);
    public void Death();
}
