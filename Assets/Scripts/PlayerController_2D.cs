using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController_2D : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField, Header("プレイヤーの移動速度")]
    private float _speed = 5.0f;

    [SerializeField, Header("プレイヤーのジャンプ力")]
    private float _jumpPower = 5.0f;

    [SerializeField, Header("プレイヤーのHP")]
    private int _hp;

    //入力の値を保存する変数
    private float _inputValueX = 0;

    //ジャンプの回数
    private int _jumpNum = 0;

    [SerializeField, Header("ジャンプできる回数")]
    private int _canJumpNum = 1;

    [Header("～インプット関係～")]
    [SerializeField, Header("横移動のキーコン")]
    private InputAction _moveXAction;

    [SerializeField, Header("ジャンプのキーコン")]
    private InputAction _jumpAction;

    // 有効化
    private void OnEnable()
    {
        // InputActionを有効化
        _moveXAction?.Enable();
        _jumpAction?.Enable();
    }

    // 無効化
    private void OnDisable()
    {
        // 自身が無効化されるタイミングなどで
        _moveXAction?.Disable();
        _jumpAction?.Disable();
    }

    void Start()
    {
        //リジッドボディを取得
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        //X軸のキー入力を保存
        _inputValueX = _moveXAction.ReadValue<float>();

        //ジャンプのボタンが押されたら
        if(_jumpAction.WasPressedThisFrame())
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
        _rb.velocity = new Vector3(_speed * _inputValueX, _rb.velocity.y, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _jumpNum = 0;
        }
    }

    //ダメージ
    public void HitDamage()
    {
        //HPを減らす
        _hp--;
    }
}
