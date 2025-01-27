using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemy : MonoBehaviour
{
    private Rigidbody _rb;

    GameObject player;

    //アニメーター
    [SerializeField]
    private Animator _animator;

    //攻撃のアニメーションの判定
    private string _isJump = "JumpNow";

    [SerializeField, Header("プレイヤーとの距離")]
    private float _distance = 5f;

    [SerializeField, Header("ジャンプ力")]
    private float _jumpPower = 5.0f;

    //ジャンプの回数
    private int _jumpNum = 0;

    [SerializeField, Header("ジャンプできる回数")]
    private int _canJumpNum = 1;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        _rb = GetComponent<Rigidbody>();
        _animator.SetBool(_isJump, false);
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            float _dis = Vector3.Distance(this.transform.position, player.transform.position);
            Jump(_dis);
        }

        if (transform.position.x > player.transform.position.x)
        {
            // X軸がプレイヤーより小さい場合、スケールを反転
            transform.localScale = new Vector3(50f, 50f, 50f);
        }
        else
        {
            // 元のスケールに戻す
            transform.localScale = new Vector3(50f, 50f, -50f);
        }
    }

    void Jump(float dis)
    {
        if (dis <= _distance)
        {
            //ジャンプする
            if (_jumpNum < _canJumpNum)
            {
                _rb.AddForce(0f, _jumpPower, 0f, ForceMode.Impulse);
                _jumpNum++;
                //ジャンプのアニメーション
                Invoke("JumpAnimation", 0.5f);
            }
        }
        else // プレイヤーとの距離が設定した範囲を超えた場合
        {
            _animator.SetBool(_isJump, false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _jumpNum = 0;
        }
    }

    void JumpAnimation()
    {
        _animator.SetBool(_isJump, true);
    }
}
