using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow3D : MonoBehaviour
{
    public GameObject _player; // �v���C���[�i�^�[�Q�b�g�j�ƂȂ�I�u�W�F�N�g
    private PlayerController_3D _playerController;
    private GameManager _gm;
    public float _distance = 5.0f; // �^�[�Q�b�g����̋���
    public float _rotationSpeed = 100.0f; // ��]���x
    public Vector2 _pitchLimits = new Vector2(-30, 60); // �㉺�̊p�x����
    public Vector3 _playerOffset = new Vector3(0, 1.5f, 0); // �v���C���[�ʒu�����p�̃I�t�Z�b�g

    private float _currentYaw = 0.0f; // ���݂̐����p�x
    private float _currentPitch = 0.0f; // ���݂̐����p�x

    [SerializeField]
    private InputAction _cameraRotateAction;

    private void OnEnable()
    {
        _cameraRotateAction?.Enable();
    }

    private void OnDisable()
    {
        _cameraRotateAction?.Disable();
    }

    void Start()
    {
        if (_player == null)
        {
            Debug.LogError("Target is not assigned! Please assign a target for the camera.");
        }
        _playerController = _player.GetComponent<PlayerController_3D>();
        _gm = FindAnyObjectByType<GameManager>();
    }

    void Update()
    {
        if (_player == null) return;

        // �}�E�X���͂���p�x���v�Z
        float mouseX = _cameraRotateAction.ReadValue<Vector2>().x * _rotationSpeed * Time.deltaTime;
        float mouseY = _cameraRotateAction.ReadValue<Vector2>().y * _rotationSpeed * Time.deltaTime;

        _currentYaw += mouseX;
        _currentPitch -= mouseY;

        // �����p�x�ɐ�����������
        _currentPitch = Mathf.Clamp(_currentPitch, _pitchLimits.x, _pitchLimits.y);

        // �J�����̈ʒu�Ɖ�]���X�V
        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        // �v���C���[�̒��S�ʒu�ɃI�t�Z�b�g��ǉ�
        Vector3 targetPosition = _player.transform.position + _playerOffset;

        // �J�����̈ʒu���^�[�Q�b�g�̎��͂ɐݒ�
        Quaternion rotation = Quaternion.Euler(5, _currentYaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -_distance);
        transform.position = targetPosition + offset;

        // �J�������^�[�Q�b�g�������悤�ɐݒ�
        transform.LookAt(targetPosition);
    }
}
