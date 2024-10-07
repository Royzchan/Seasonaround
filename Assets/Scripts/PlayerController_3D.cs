using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController_3D : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField, Header("�v���C���[�̈ړ����x")]
    private float _speed = 5.0f;

    [SerializeField, Header("�v���C���[�̃W�����v��")]
    private float _jumpPower = 5.0f;

    //���͂̒l��ۑ�����ϐ�
    private float _inputValueX = 0;

    private float _inputValueZ = 0;

    //�W�����v�̉�
    private int _jumpNum = 0;

    [SerializeField, Header("�W�����v�ł����")]
    private int _canJumpNum = 1;

    [Header("�`�C���v�b�g�֌W�`")]
    [SerializeField, Header("���ړ��̃L�[�R��")]
    private InputAction _moveXAction;

    [SerializeField, Header("�c�ړ��̃L�[�R��")]
    private InputAction _moveZAction;

    [SerializeField, Header("�W�����v�̃L�[�R��")]
    private InputAction _jumpAction;

    // �L����
    private void OnEnable()
    {
        // InputAction��L����
        _moveXAction?.Enable();
        _moveZAction?.Enable();
        _jumpAction?.Enable();
    }

    // ������
    private void OnDisable()
    {
        // ���g�������������^�C�~���O�Ȃǂ�
        _moveXAction?.Disable();
        _moveZAction?.Disable();
        _jumpAction?.Disable();
    }

    void Start()
    {
        //���W�b�h�{�f�B���擾
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //���̈ړ��̓��͂̒l��ۑ�
        _inputValueX = _moveXAction.ReadValue<float>();

        //�c�̈ړ��̓��͂̒l��ۑ�
        _inputValueZ = _moveZAction.ReadValue<float>();

        //�W�����v�L�[�������ꂽ��
        if (_jumpAction.WasPressedThisFrame())
        {
            //�W�����v����
            if (_jumpNum < _canJumpNum)
            {
                _rb.AddForce(0f, _jumpPower, 0f, ForceMode.Impulse);
                _jumpNum++;
            }
        }
    }

    private void FixedUpdate()
    {
        //���x�����ړ��̒l��
        _rb.velocity = new Vector3(_speed * _inputValueX, _rb.velocity.y, _speed * _inputValueZ);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _jumpNum = 0;
        }
    }
}
