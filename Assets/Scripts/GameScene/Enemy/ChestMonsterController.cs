using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestMonsterController : MonoBehaviour
{
    public enum State
    {
        Idle,   //待機
        Run,    //移動
        Attack, //攻撃
        Search  //探す
    }

    private Rigidbody _rb;
    private GameObject _player;
    private Animator _animator;

    private State _state = State.Idle;

    [SerializeField, Header("移動速度")]
    private float _speed = 0.5f;
    [SerializeField, Header("攻撃を始める距離")]
    private float _attackDistance = 2.0f;
    [SerializeField, Header("何秒動いたら攻撃するか")]
    private float _attackTime = 1.5f;
    [SerializeField, Header("攻撃後敵を探す時間")]
    private float _serachPlayerTime = 2.0f;

    private bool _foundPlayer = false;    //プレイヤーを見つけているか
    private float _runTimeCount = 0.0f;   //プレイヤー発見後走る時間
    private float _searchTimeCount = 0.0f;//プレイヤーを探す時間
    private float _moveDirection = 1.0f;  //プレイヤーの進む向き

    //アニメーションのstring
    private string _isIdle = "isIdleChest";
    private string _isDiscovery = "isDiscovery";
    private string _isRun = "isRun";
    private string _isAttack = "isAttack";
    private string _isSearch = "isSearch";

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GameObject.FindWithTag("Player");
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        switch (_state)
        {
            case State.Idle:
                CheckPlayerDistance();
                break;

            case State.Run:
                _runTimeCount += Time.deltaTime;
                if (_runTimeCount >= _attackTime)
                {
                    AtackStart();
                }
                _rb.velocity = new Vector3(_moveDirection * _speed, _rb.velocity.y, 0f);
                break;

            case State.Attack:
                break;

            case State.Search:
                if (!_foundPlayer)
                {
                    _searchTimeCount += Time.deltaTime;
                    if (_searchTimeCount >= _serachPlayerTime)
                    {
                        _animator.SetBool(_isSearch, false);
                        _searchTimeCount = 0;
                        if (!CheckPlayerDistance())
                        {
                            ChengeState(State.Idle);
                            _animator.SetBool(_isIdle, true);
                        }
                        else
                        {
                            FoundPlayer();
                        }
                    }
                }
                break;
        }
    }

    private bool CheckPlayerDistance()
    {
        if (!_foundPlayer)
        {
            var playerPos = _player.transform.position;
            var myPos = this.transform.position;
            var distance = Vector3.Distance(myPos, playerPos);

            if (distance <= _attackDistance)
            {
                if ((myPos.x - playerPos.x) <= 0)
                {
                    LookRight();
                }
                else
                {
                    LookLeft();
                }

                FoundPlayer();
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    private void ChengeState(State state)
    {
        _state = state;
    }

    public void FoundPlayer()
    {
        _foundPlayer = true;
        _animator.SetBool(_isDiscovery, true);
    }

    public void RunStart()
    {
        ChengeState(State.Run);
        _animator.SetBool(_isDiscovery, false);
        _animator.SetBool(_isRun, true);
    }

    public void AtackStart()
    {
        ChengeState(State.Attack);
        _animator.SetBool(_isRun, false);
        _animator.SetBool(_isAttack, true);
    }

    public void AttackEnd()
    {
        ChengeState(State.Search);
        _animator.SetBool(_isAttack, false);
        _animator.SetBool(_isSearch, true);
        _foundPlayer = false;
        _runTimeCount = 0f;
    }

    public void LookRight()
    {
        transform.eulerAngles = new Vector3(0, 90f, 0);
        _moveDirection = 1;
    }

    public void LookLeft()
    {
        transform.eulerAngles = new Vector3(0, -90f, 0);
        _moveDirection = -1;
    }

    // 追記：プレイヤーに踏まれた際の処理
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーが上から踏みつけたかを判定する
            if (collision.transform.position.y > transform.position.y + 0.5f) // 0.5fは踏みつけ判定の高さの閾値
            {
                Die();
            }
        }
    }

    private void Die()
    {
        // 死亡時の処理を記述（例：オブジェクトを非アクティブにする、破壊する等）
        Destroy(gameObject);
        Debug.Log("宝箱モンスターが踏みつけられて死んだ！");
    }
}