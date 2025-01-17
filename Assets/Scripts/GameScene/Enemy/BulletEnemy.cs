using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    GameObject _player;
    private Animator _animator;

    [SerializeField, Header("攻撃を始める距離")]
    private float _attackDistance = 5f;
    [SerializeField, Header("攻撃の間隔")]
    private float _attackCoolTime = 3.0f;
    private float _coolTimeCount = 0;

    [SerializeField, Header("弾のオブジェクト")]
    private GameObject bulletPrefab;

    [SerializeField, Header("弾の生成場所")]
    private Transform bulletPosition;

    private bool _canAttack = true;

    //ア二メーションのステータス
    private string _attackStr = "isAttack";

    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!_canAttack)
        {
            _coolTimeCount += Time.deltaTime;
            if (_coolTimeCount >= _attackCoolTime)
            {
                _coolTimeCount = 0;
                _canAttack = true;
            }
        }

        if (_player != null)
        {
            // プレイヤーと敵の位置関係用
            float _compare = this.transform.position.x - _player.transform.position.x;
            LeftorRight(_compare);

            CheckPlayerDistance();
        }
    }

    //攻撃
    public void Attack()
    {
        //弾を生成
        Instantiate(bulletPrefab, bulletPosition.position, bulletPosition.rotation);
    }

    //距離チェック
    private void CheckPlayerDistance()
    {
        var myPos = this.transform.position;
        var playerPos = _player.transform.position;
        var distance = Vector3.Distance(myPos, playerPos);
        if (distance <= _attackDistance)
        {
            if (_canAttack)
            {
                _animator.SetBool(_attackStr, true);
                _canAttack = false;
                _coolTimeCount = 0;
            }
        }
    }

    // 左向きか右向きか
    void LeftorRight(float compare)
    {
        if (compare > 0)
        {
            this.transform.eulerAngles = new Vector3(0, -90, 0);
        }
        else
        {
            this.transform.eulerAngles = new Vector3(0, 90, 0);
        }
    }

    public void EndAttackAnim()
    {
        _animator.SetBool(_attackStr, false);
    }
}
