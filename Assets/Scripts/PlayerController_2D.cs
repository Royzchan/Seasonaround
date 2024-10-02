using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_2D : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField, Header("�v���C���[�̈ړ����x")]
    private float _speed = 5.0f;

    [SerializeField, Header("�v���C���[�̃W�����v��")]
    private float _jumpPower = 5.0f;

    //���͂̒l��ۑ�����ϐ�
    private float _inputValueX = 0;

    //�W�����v�̉�
    private int _jumpNum = 0;

    [SerializeField, Header("�W�����v�ł����")]
    private int _canJumpNum = 1;

    void Start()
    {
        //���W�b�h�{�f�B���擾
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //�E�ړ��̃L�[��������Ă����
        if (Input.GetKey(KeyCode.D))
        {
            //���ړ��̓��͕ۑ��̐��l��1
            _inputValueX = 1.0f;
        }
        //���ړ��̃L�[��������Ă����
        else if (Input.GetKey(KeyCode.A))
        {
            //���ړ��̓��͂̐��l��-1
            _inputValueX = -1.0f;
        }
        //�ړ��֌W�̃L�[��������Ă��Ȃ��ꍇ
        else
        {
            //���ړ��̓��͂̐��l��0
            _inputValueX = 0f;
        }

        //�W�����v�L�[�������ꂽ��
        if (Input.GetKeyDown(KeyCode.Space))
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
        _rb.velocity = new Vector3(_speed * _inputValueX, _rb.velocity.y, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _jumpNum = 0;
        }
    }
}
