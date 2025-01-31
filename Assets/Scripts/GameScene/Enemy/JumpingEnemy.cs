using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class JumpingEnemy : MonoBehaviour
{
    private Rigidbody _rb;
    GameObject _player;
    PlayerController_2D _playerController;
    private Animator _animator;

    [SerializeField, Header("プレイヤーとの距離")]
    private float _distance = 5f;
    [SerializeField, Header("ジャンプ力")]
    private float _jumpPower = 5.0f;
    [SerializeField, Header("ジャンプできる回数")]
    private int _canJumpNum = 1;
    [SerializeField, Header("ジャンプのクールタイム")]
    private float _jumpCoolTime = 1.0f;
    private int _jumpNum = 0; // ジャンプした回数
    private float _jumpCoolTimeCounter = 0f; // ジャンプのクールタイムのカウンター
    private bool _canJump = true; // ジャンプ可能フラグ
    private bool _onGround = true;

    //アニメーション
    private string _isJump = "Jump"; // ジャンプのアニメーションの呼び出しキー
    private string _isDie = "Die";   // 死亡のアニメーションの呼び出しキー

    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _playerController = FindAnyObjectByType<PlayerController_2D>();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player != null)
        {
            float dis = Vector3.Distance(this.transform.position, _player.transform.position);
            if (dis <= _distance)
            {
                if (_canJump) Jump();
            }

            if (transform.position.x > _player.transform.position.x)
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
    }

    /// <summary>
    /// ジャンプ処理
    /// </summary>
    void Jump()
    {
        if (_jumpNum < _canJumpNum && _onGround)
        {
            _canJump = false;
            StartCoroutine(StartJumpCoolTime());
            _animator.SetTrigger(_isJump);
        }
    }

    /// <summary>
    /// ジャンプの力を加える処理
    /// </summary>
    public void AddJumpForce()
    {
        _rb.AddForce(0f, _jumpPower, 0f, ForceMode.Impulse);
    }

    private IEnumerator StartJumpCoolTime()
    {
        yield return new WaitForSeconds(_jumpCoolTime);
        _canJump = true;
    }

    private void Die()
    {
        _animator.SetTrigger(_isDie);
    }

    public void DeleteObj()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //接地したタイミングで地面にいる判定をtrueに
            _onGround = true;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            if (_player.transform.position.y >= this.transform.position.y)
            {
                Die();
            }
            else
            {
                if (_playerController != null) _playerController.Damage(1);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //地面から離れたら地面にいる判定をfalseに
            _onGround = false;
        }
    }
}
