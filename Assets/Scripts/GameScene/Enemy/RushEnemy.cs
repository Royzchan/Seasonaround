using UnityEngine;

public class RushEnemy : MonoBehaviour
{
    GameObject player;
    Rigidbody rb;

    //アニメーター
    [SerializeField]
    private Animator _animator;

    //攻撃のアニメーションの判定
    private string _isAttack = "AttackNow";

    [SerializeField, Header("プレイヤーとの距離")]
    private float _distance = 5f;

    [SerializeField, Header("突進速度")]
    private float _rushSpeed = 10f;

    [SerializeField, Header("プレイヤーを押し戻す力")]
    private float _knockbackPower = 10f;

    [SerializeField, Header("突進を停止する時間")]
    private float _pauseDuration = 2f;

    [SerializeField, Header("待機時間")]
    private float _waitTime = 1f;

    [SerializeField, Header("突進時間")]
    private float _rushDuration = 3f;  // 突進する時間（秒）

    private float _waitCount = 0f; // 待機時間のタイマー
    private float _pauseTimer = 0f; // 停止中のタイマー
    private float _rushTimer = 0f; // 突進中の経過時間
    private bool isRushing = false;
    private bool isStandbay = false;
    private float rushDirectionX; // X軸方向のみの突進方向
    private Vector3 rushStartPosition; // 突進開始時の位置

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        _pauseTimer = _pauseDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            float _dis = Vector3.Distance(this.transform.position, player.transform.position);
            Rush(_dis);
        }

        if (!isRushing)
        {
            if (transform.position.x > player.transform.position.x)
            {
                // X軸がプレイヤーより小さい場合、スケールを反転
                transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            }
            else
            {
                // 元のスケールに戻す
                transform.localScale = new Vector3(1.3f, 1.3f, -1.3f);
            }
        }
    }

    void GoAttack()
    {
        isRushing = true;

        // 突進方向を決定（プレイヤーの位置を基準）
        rushDirectionX = player.transform.position.x > transform.position.x ? 1f : -1f;

        print("突進開始");
    }

    // 突進関数
    void Rush(float dis)
    {
        if (dis <= _distance)
        {
            // 一時停止タイマーの更新
            if (_pauseTimer > 0f)
            {
                _pauseTimer -= Time.deltaTime; // タイマーを減らす
            }

            if (!isRushing && _pauseTimer <= 0f) // 一時停止中でない場合に突進を開始
            {
                //突進のアニメーション開始
                _animator.SetBool(_isAttack, true);
                Invoke("GoAttack", 1f);
            }
        }
        else // プレイヤーとの距離が設定した範囲を超えた場合
        {
            isRushing = false;
            _animator.SetBool(_isAttack, false);
            _pauseTimer = _pauseDuration; // 一時停止タイマーを設定
        }

        // 突進中は最初に決めた方向に向かって移動し続ける
        if (isRushing)
        {
            // 突進時間が経過したら突進を停止
            if (_rushTimer >= _rushDuration)
            {
                isRushing = false;
                _pauseTimer = _pauseDuration; // 一時停止タイマーを設定
                print("突進終了（時間経過）");

                // プレイヤーの位置に基づいて突進方向を決定（最初の一度だけ)
                rushDirectionX = player.transform.position.x > transform.position.x ? 1f : -1f;
                rushStartPosition = transform.position; // 突進開始時の位置を記録
                _rushTimer = 0f; // 突進時間のリセット
            }
            else
            {
                // 突進中に経過時間を増加
                _rushTimer += Time.deltaTime;
                transform.position += new Vector3(rushDirectionX * _rushSpeed * Time.deltaTime, 0f, 0f);
            }
        }
    }

    // 衝突時の処理
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 突進を停止する
            isRushing = false;
            _pauseTimer = _pauseDuration; // 一時停止タイマーを設定

            // プレイヤーが後ろに少し下がる
            Vector3 direction = (player.transform.position - transform.position).normalized;
            player.transform.position += direction * _knockbackPower * Time.deltaTime;
        }
    }
}
