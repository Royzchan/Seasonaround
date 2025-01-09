using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestMonsterController : MonoBehaviour
{
    public enum State
    {
        Idle,   //�ҋ@
        Move,   //�ړ�
        Attack  //�U��
    }

    private Rigidbody _rb;
    private GameObject _player;
    private Animator _animator;

    private State _state = State.Idle;

    [SerializeField, Header("�ړ����x")]
    private float _speed = 5.0f;
    [SerializeField, Header("�ړ����n�߂鋗��")]
    private float _attackDistance = 2.0f;
    [SerializeField, Header("���b��������U�����邩")]
    private float _attackTime = 2.0f;

    //�A�j���[�V������string
    private string _idleChestStr;
    private string _idleNormalStr;
    private string _walkStr;
    private string _runStr;

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

            case State.Move:
                break;

            case State.Attack:
                break;
        }
    }

    private void CheckPlayerDistance()
    {
        var distance = Vector3.Distance(_player.transform.position, this.transform.position);
        if (distance <= _attackDistance)
        {
            ChengeState(State.Move);
        }
    }

    private void ChengeState(State state)
    {
        _state = state;
    }
}
