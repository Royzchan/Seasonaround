using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


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

public class PlayerController_2D : MonoBehaviour
{
    private Rigidbody _rb;

    //���݂Ȃ�̓������ǂ���
    private Animal _nowAnimal = Animal.Normal;

    [SerializeField, Header("�v���C���[�̈ړ����x")]
    private float _speed = 5.0f;

    [SerializeField, Header("�v���C���[�̃W�����v��")]
    private float _jumpPower = 5.0f;

    [SerializeField, Header("�v���C���[��HP")]
    private int _hp;

    //���͂̒l��ۑ�����ϐ�
    private float _inputValueX = 0;

    //�W�����v�̉�
    private int _jumpNum = 0;

    [SerializeField, Header("�W�����v�ł����")]
    private int _canJumpNum = 1;

    //�W�����v�����ǂ����̔���
    private bool _jumpNow = false;

    //�G�ꂽ�G��|���邩�ǂ���
    private bool _canDefeatEnemy = false;

    [SerializeField, Header("���͌���(�|�����)"), Range(0.9f, 0.99f)]
    private float _windLatePower;
    private Vector3 _windPower;

    [SerializeField, Header("�����̃��f��")]
    private GameObject[] _animalModels;

    [Header("�`�C���v�b�g�֌W�`")]
    [SerializeField, Header("���ړ��̃L�[�R��")]
    private InputAction _moveXAction;

    [SerializeField, Header("�W�����v�̃L�[�R��")]
    private InputAction _jumpAction;

    // �L����
    private void OnEnable()
    {
        // InputAction��L����
        _moveXAction?.Enable();
        _jumpAction?.Enable();
    }

    // ������
    private void OnDisable()
    {
        // ���g�������������^�C�~���O�Ȃǂ�
        _moveXAction?.Disable();
        _jumpAction?.Disable();
    }

    void Start()
    {
        //���W�b�h�{�f�B���擾
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        //X���̃L�[���͂�ۑ�
        _inputValueX = _moveXAction.ReadValue<float>();

        //�W�����v�̃{�^���������ꂽ��
        if (_jumpAction.WasPressedThisFrame())
        {
            //�W�����v�����񐔂��W�����v�\�񐔂��Ⴉ������
            if (_jumpNum < _canJumpNum)
            {
                //��ɗ͂�������
                _rb.AddForce(0f, _jumpPower, 0f, ForceMode.Impulse);
                //�W�����v�����񐔂��v���X
                _jumpNum++;
                //�W�����v���̔����true��
                _jumpNow = true;
            }
        }

        //�W�����v����������
        if (_jumpNow)
        {
            // Ray���v���C���[�̑O���ɔ�΂�
            Ray ray = new Ray(transform.position, -transform.up);

            RaycastHit hit;

            // ���C�L���X�g�����s���āA�����ɓ������������m�F
            if (Physics.Raycast(ray, out hit))
            {
                //���C���G�ɓ������Ă�����
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    //�G��|����悤��
                    _canDefeatEnemy = true;
                }
                //�Ⴄ�����ɓ������Ă�����
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
        }
    }

    private void FixedUpdate()
    {
        //���x�����ړ��̒l��
        _rb.velocity = new Vector3((_speed * _inputValueX), _rb.velocity.y, 0f) + _windPower;
        WindPowDown();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //�G��|���锻�肪true��������
            if (_canDefeatEnemy)
            {
                Destroy(collision.gameObject);
            }
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            _jumpNum = 0;
            _jumpNow = false;
        }
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

    //�����̌ŗL�A�N�V����
    private void AnimalAction()
    {

    }

    //�����ɐG�ꂽ���ɕԐM���鏈��
    public void ChangeAnimal(Animal animal)
    {
        //���f���̔z����m�F���Ă���
        for (int i = 0; i < _animalModels.Length; i++)
        {
            //�����ԐM���铮���̃��f���ɂȂ�����
            if (i == (int)animal)
            {
                //���̓����̃��f�����Z�b�g
                _animalModels[i].SetActive(true);
            }
            //����ȊO�̓����������ꍇ
            else
            {
                //�����̃��f��������
                _animalModels[i].SetActive(false);
            }
        }
    }

    private void SetState()
    {
        _speed = AnimalData.animalDatas[0].Speed;
        _jumpPower = AnimalData.animalDatas[0].JumpPower;
    }
}
