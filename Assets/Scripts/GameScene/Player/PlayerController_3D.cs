using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController_3D : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField, Header("プレイヤーの移動速度")]
    private float _speed = 5.0f;

    [SerializeField, Header("プレイヤーのジャンプ力")]
    private float _jumpPower = 5.0f;

    // 入力の値を保存する変数
    private Vector2 _inputVector = Vector2.zero;

    // ジャンプの回数
    private int _jumpNum = 0;

    [SerializeField, Header("ジャンプできる回数")]
    private int _canJumpNum = 1;

    [Header("～インプット関係～")]
    [SerializeField, Header("移動のキーコン")]
    private InputAction _moveAction;

    [SerializeField, Header("ジャンプのキーコン")]
    private InputAction _jumpAction;

    [SerializeField, Header("プレイヤーを追従するカメラ")]
    private Transform _cameraTransform;

    // 有効化
    private void OnEnable()
    {
        _moveAction?.Enable();
        _jumpAction?.Enable();
    }

    // 無効化
    private void OnDisable()
    {
        _moveAction?.Disable();
        _jumpAction?.Disable();
    }

    void Start()
    {
        // リジッドボディを取得
        _rb = GetComponent<Rigidbody>();

        // カメラの参照が設定されていない場合、メインカメラを取得
        if (_cameraTransform == null)
        {
            _cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        // 移動入力の値を保存
        _inputVector = _moveAction.ReadValue<Vector2>();

        // ジャンプキーが押されたら
        if (_jumpAction.WasPressedThisFrame() && _jumpNum < _canJumpNum)
        {
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
            _jumpNum++;
        }
    }

    private void FixedUpdate()
    {
        // カメラの前方向と右方向を取得
        Vector3 cameraForward = _cameraTransform.forward;
        Vector3 cameraRight = _cameraTransform.right;

        // Y軸成分を除去（地面平面に沿った移動のため）
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // 移動ベクトルを計算
        Vector3 moveDirection = cameraRight * _inputVector.x + cameraForward * _inputVector.y;
        moveDirection *= _speed;

        // プレイヤーの速度を設定
        _rb.velocity = new Vector3(moveDirection.x, _rb.velocity.y, moveDirection.z);

        // プレイヤーの回転を移動方向に合わせる
        if (moveDirection.sqrMagnitude > 0.01f) // わずかな入力では回転させない
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f); // スムーズに回転
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
