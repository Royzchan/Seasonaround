using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestMonsterController : MonoBehaviour
{
    public enum State
    {
        Idle,   //待機
        Move,   //移動
        Attack  //攻撃
    }

    private Rigidbody _rb;
    private GameObject _player;
    private Animator _animator;

    private State _state = State.Idle;

    [SerializeField, Header("移動速度")]
    private float _speed = 5.0f;
    [SerializeField, Header("移動を始める距離")]
    private float _attackDistance = 2.0f;
    [SerializeField, Header("何秒動いたら攻撃するか")]
    private float _attackTime = 2.0f;

    //アニメーションのstring
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
