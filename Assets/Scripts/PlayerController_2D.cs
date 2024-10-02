using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_2D : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField, Header("プレイヤーの移動速度")]
    private float _speed = 5.0f;

    [SerializeField, Header("プレイヤーのジャンプ力")]
    private float _jumpPower = 5.0f;

    //入力の値を保存する変数
    private float _inputValueX = 0;

    //ジャンプの回数
    private int _jumpNum = 0;

    [SerializeField, Header("ジャンプできる回数")]
    private int _canJumpNum = 1;

    void Start()
    {
        //リジッドボディを取得
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //右移動のキーが押されている間
        if (Input.GetKey(KeyCode.D))
        {
            //横移動の入力保存の数値を1
            _inputValueX = 1.0f;
        }
        //左移動のキーが押されている間
        else if (Input.GetKey(KeyCode.A))
        {
            //横移動の入力の数値を-1
            _inputValueX = -1.0f;
        }
        //移動関係のキーが押されていない場合
        else
        {
            //横移動の入力の数値を0
            _inputValueX = 0f;
        }

        //ジャンプキーが押されたら
        if (Input.GetKeyDown(KeyCode.Space))
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
}
