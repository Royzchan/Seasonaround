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

    // アニメーションのステータス
    private string _attackStr = "isAttack";

    // 待機中の上下の動きに関する変数
    [SerializeField, Header("待機中の上下移動の範囲")]
    private float _moveRange = 0.5f;
    [SerializeField, Header("上下移動の速さ")]
    private float _moveSpeed = 2f;
    private float _initialY;

    private bool isAttacking = false;  // 攻撃中かどうかのフラグ

    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _animator = GetComponent<Animator>();
        _initialY = transform.position.y; // 初期Y座標を保存
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

        MoveUpAndDown();
    }

    // 攻撃
    public void Attack()
    {
        // 弾を生成
        Instantiate(bulletPrefab, bulletPosition.position, bulletPosition.rotation);
        isAttacking = true;  // 攻撃中フラグをオン
    }

    // 距離チェック
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
        isAttacking = false;  // 攻撃終了時にフラグをオフ
    }

    // 待機中の上下移動処理
    private void MoveUpAndDown()
    {
        float newY = _initialY + Mathf.Sin(Time.time * _moveSpeed) * _moveRange;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    // プレイヤーに踏まれたときの処理
    private void OnCollisionEnter(Collision collision)
    {
        // プレイヤーが敵に接触した場合
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーが敵の上に乗ったかどうかを確認
            if (collision.contacts[0].point.y > transform.position.y)
            {
                Die();  // 上から踏まれた場合は敵が死ぬ
            }
        }
    }

    // 敵が死亡する処理
    private void Die()
    {
        Destroy(gameObject);  // 敵を消す
    }
}
