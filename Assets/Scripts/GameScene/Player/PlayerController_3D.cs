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

    // ���͂̒l��ۑ�����ϐ�
    private Vector2 _inputVector = Vector2.zero;

    // �W�����v�̉�
    private int _jumpNum = 0;

    [SerializeField, Header("�W�����v�ł����")]
    private int _canJumpNum = 1;

    [Header("�`�C���v�b�g�֌W�`")]
    [SerializeField, Header("�ړ��̃L�[�R��")]
    private InputAction _moveAction;

    [SerializeField, Header("�W�����v�̃L�[�R��")]
    private InputAction _jumpAction;

    [SerializeField, Header("�v���C���[��Ǐ]����J����")]
    private Transform _cameraTransform;

    // �L����
    private void OnEnable()
    {
        _moveAction?.Enable();
        _jumpAction?.Enable();
    }

    // ������
    private void OnDisable()
    {
        _moveAction?.Disable();
        _jumpAction?.Disable();
    }

    void Start()
    {
        // ���W�b�h�{�f�B���擾
        _rb = GetComponent<Rigidbody>();

        // �J�����̎Q�Ƃ��ݒ肳��Ă��Ȃ��ꍇ�A���C���J�������擾
        if (_cameraTransform == null)
        {
            _cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        // �ړ����͂̒l��ۑ�
        _inputVector = _moveAction.ReadValue<Vector2>();

        // �W�����v�L�[�������ꂽ��
        if (_jumpAction.WasPressedThisFrame() && _jumpNum < _canJumpNum)
        {
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
            _jumpNum++;
        }
    }

    private void FixedUpdate()
    {
        // �J�����̑O�����ƉE�������擾
        Vector3 cameraForward = _cameraTransform.forward;
        Vector3 cameraRight = _cameraTransform.right;

        // Y�������������i�n�ʕ��ʂɉ������ړ��̂��߁j
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // �ړ��x�N�g�����v�Z
        Vector3 moveDirection = cameraRight * _inputVector.x + cameraForward * _inputVector.y;
        moveDirection *= _speed;

        // �v���C���[�̑��x��ݒ�
        _rb.velocity = new Vector3(moveDirection.x, _rb.velocity.y, moveDirection.z);

        // �v���C���[�̉�]���ړ������ɍ��킹��
        if (moveDirection.sqrMagnitude > 0.01f) // �킸���ȓ��͂ł͉�]�����Ȃ�
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f); // �X���[�Y�ɉ�]
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _jumpNum = 0;
        }
    }
}
