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

    //入力の値を保存する変数
    private float _inputValueX = 0;

    private float _inputValueZ = 0;

    //ジャンプの回数
    private int _jumpNum = 0;

    [SerializeField, Header("ジャンプできる回数")]
    private int _canJumpNum = 1;

    [Header("～インプット関係～")]
    [SerializeField, Header("横移動のキーコン")]
    private InputAction _moveXAction;

    [SerializeField, Header("縦移動のキーコン")]
    private InputAction _moveZAction;

    [SerializeField, Header("ジャンプのキーコン")]
    private InputAction _jumpAction;

    // 有効化
    private void OnEnable()
    {
        // InputActionを有効化
        _moveXAction?.Enable();
        _moveZAction?.Enable();
        _jumpAction?.Enable();
    }

    // 無効化
    private void OnDisable()
    {
        // 自身が無効化されるタイミングなどで
        _moveXAction?.Disable();
        _moveZAction?.Disable();
        _jumpAction?.Disable();
    }

    void Start()
    {
        //リジッドボディを取得
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //横の移動の入力の値を保存
        _inputValueX = _moveXAction.ReadValue<float>();

        //縦の移動の入力の値を保存
        _inputValueZ = _moveZAction.ReadValue<float>();

        //ジャンプキーが押されたら
        if (_jumpAction.WasPressedThisFrame())
        {
            //ジャンプする
            if (_jumpNum < _canJumpNum)
            {
                _rb.AddForce(0f, _jumpPower, 0f, ForceMode.Impulse);
                _jumpNum++;
            }
        }
    }

    private void FixedUpdate()
    {
        //速度を横移動の値に
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
