using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestMonsterController : MonoBehaviour
{
    public enum State
    {
        Idle,   //‘Ò‹@
        Run,    //ˆÚ“®
        Attack, //UŒ‚
        Search  //’T‚·
    }

    private Rigidbody _rb;
    private GameObject _player;
    private Animator _animator;

    private State _state = State.Idle;

    [SerializeField, Header("ˆÚ“®‘¬“x")]
    private float _speed = 5.0f;
    [SerializeField, Header("UŒ‚‚ğn‚ß‚é‹——£")]
    private float _attackDistance = 2.0f;
    [SerializeField, Header("‰½•b“®‚¢‚½‚çUŒ‚‚·‚é‚©")]
    private float _attackTime = 1.5f;
    [SerializeField, Header("UŒ‚Œã“G‚ğ’T‚·ŠÔ")]
    private float _serachPlayerTime = 2.0f;

    private bool _foundPlayer = false;
    private float _runTimeCount = 0.0f;
    private float _searchTimeCount = 0.0f;

    //ƒAƒjƒ[ƒVƒ‡ƒ“‚Ìstring
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
}
