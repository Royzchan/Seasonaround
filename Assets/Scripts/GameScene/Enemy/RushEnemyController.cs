using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushEnemyController : MonoBehaviour
{

    [SerializeField, Header("移動スピード")]
    private float _speed;
    [SerializeField, Header("突進を始める距離")]
    private float _attackDistance;
    [SerializeField, Header("突進を行う時間"), Range(0.1f, 2.0f)]
    private float _attackTime = 2.0f;
    private float _rushTimeCount = 0f;

    private Rigidbody _rb; // リジッドボディ
    private GameObject _player; // プレイヤー
    private Animator _animator; // アニメーター

    private bool _rushing = false; // 突進中のフラグ
    private bool _checkPlayerDis = true; // プレイヤーの距離を確認中かのフラグ
    private float _checkCoolTime = 5.0f; // 突進後再度突進するまでのクールタイム

    //アニメーション関係
    private string _attackStr = "Attack";


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null) Debug.LogError("RigidBodyが登録されていません。");

        //プレイヤーを見つけて登録
        _player = FindAnyObjectByType<PlayerController_2D>().gameObject;
        if (_player == null) Debug.LogError("プレイヤーが登録されていません。");

        _animator = GetComponent<Animator>();
        if (_animator == null) Debug.LogError("アニメーターが登録されていません。");
    }

    void Update()
    {
        if (_rushing)
        {
            _rb.velocity = Vector3.forward * _speed;
            _rushTimeCount += Time.deltaTime;
            if (_rushTimeCount >= _attackTime)
            {
                _rushing = false;
                _rushTimeCount = 0;
            }
        }
        else
        {
            _rb.velocity = Vector3.zero;
            CheckPlayerDistance();
        }
    }

    /// <summary>
    /// プレイヤーとの距離をチェックする
    /// </summary>
    private void CheckPlayerDistance()
    {
        if (!_checkPlayerDis) return;

        var distance = Vector3.Distance(this.transform.position, _player.transform.position);

        if (distance <= _attackDistance)
        {
            if (!_rushing)
            {
                _animator.SetTrigger(_attackStr);
                _checkPlayerDis = false;
                StartCoroutine(CheckPlayerCoolTime());
            }
        }
    }

    private IEnumerator CheckPlayerCoolTime()
    {
        yield return new WaitForSeconds(_checkCoolTime);
        _checkPlayerDis = true;
    }

    public void Rush()
    {
        _rushing = true;
    }
}
