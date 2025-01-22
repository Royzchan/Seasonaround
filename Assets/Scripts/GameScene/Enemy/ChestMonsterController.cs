using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestMonsterController : MonoBehaviour
{
    public enum State
    {
        Idle,   //�ҋ@
        Run,    //�ړ�
        Attack, //�U��
        Search  //�T��
    }

    private Rigidbody _rb;
    private GameObject _player;
    private Animator _animator;

    private State _state = State.Idle;

    [SerializeField, Header("�ړ����x")]
    private float _speed = 0.5f;
    [SerializeField, Header("�U�����n�߂鋗��")]
    private float _attackDistance = 2.0f;
    [SerializeField, Header("���b��������U�����邩")]
    private float _attackTime = 1.5f;
    [SerializeField, Header("�U����G��T������")]
    private float _serachPlayerTime = 2.0f;

    private bool _foundPlayer = false;    //�v���C���[�������Ă��邩
    private float _runTimeCount = 0.0f;   //�v���C���[�����㑖�鎞��
    private float _searchTimeCount = 0.0f;//�v���C���[��T������
    private float _moveDirection = 1.0f;  //�v���C���[�̐i�ތ���

    //�A�j���[�V������string
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

    // �ǋL�F�v���C���[�ɓ��܂ꂽ�ۂ̏���
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // �v���C���[���ォ�瓥�݂������𔻒肷��
            if (collision.transform.position.y > transform.position.y + 0.5f) // 0.5f�͓��݂�����̍�����臒l
            {
                Die();
            }
        }
    }

    private void Die()
    {
        // ���S���̏������L�q�i��F�I�u�W�F�N�g���A�N�e�B�u�ɂ���A�j�󂷂铙�j
        Destroy(gameObject);
        Debug.Log("�󔠃����X�^�[�����݂����Ď��񂾁I");
    }
}