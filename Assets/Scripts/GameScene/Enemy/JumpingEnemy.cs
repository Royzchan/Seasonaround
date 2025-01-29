using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemy : MonoBehaviour
{
    private Rigidbody _rb;
    GameObject _player;
    private PlayerController_2D _playerController;

    //アニメーター
    [SerializeField]
    private Animator _animator;

    [SerializeField, Header("プレイヤーとの距離")]
    private float _distance = 5f;
    [SerializeField, Header("ジャンプ力")]
    private float _jumpPower = 5.0f;
    [SerializeField, Header("ジャンプできる回数")]
    private int _canJumpNum = 1;
    //ジャンプの回数
    private int _jumpNum = 0;
    [SerializeField, Header("ジャンプのクールタイム")]
    private float _jumpCoolTime = 1.0f;
    private bool _canJump = true;

    //攻撃のアニメーションの判定
    private string _isJump = "JumpNow";
    private string _dieStr = "Die";

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        if (_player != null) _playerController = _player.GetComponent<PlayerController_2D>();
        _rb = GetComponent<Rigidbody>();
        _animator.SetBool(_isJump, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_player != null)
        {
            float _dis = Vector3.Distance(this.transform.position, _player.transform.position);
            Jump(_dis);
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

    void Jump(float dis)
    {
        if (dis <= _distance)
        {
            //ジャンプする
            if (_jumpNum < _canJumpNum && _canJump)
            {
                _jumpNum++;
                _canJump = false;
                //ジャンプのアニメーション
                JumpAnimation();
            }
        }
    }

    public void AddJumpPower()
    {
        _rb.AddForce(0f, _jumpPower, 0f, ForceMode.Impulse);
    }

    private IEnumerator StartCoolTime()
    {
        yield return new WaitForSeconds(_jumpCoolTime);
        _jumpNum = 0;
        _canJump = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if(!_canJump)
            {
                StartCoroutine(StartCoolTime());
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            //プレイヤーが自分より上にいるか
            bool playerIsUp = _player.transform.position.y >= this.transform.position.y;
            if (playerIsUp)
            {
                //自身の死亡処理
                Die();
            }
            else
            {
                //プレイヤーのダメージ処理
            }
        }
    }

    void JumpAnimation()
    {
        _animator.SetTrigger(_isJump);
    }

    void Die()
    {
        _animator.SetTrigger(_dieStr);
    }

    public void DeleteObj()
    {
        Destroy(this.gameObject);
    }
}
