using UnityEngine;

public class RushEnemy : MonoBehaviour
{
    GameObject player;
    Rigidbody rb;

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

    private float _waitCount = 0f; // 待機時間のタイマー
    private float _pauseTimer = 0f; // 停止中のタイマー
    private bool isRushing = false;

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
                isRushing = true; // 突進を開始
                print("突進開始");
            }
        }
        else // プレイヤーとの距離が設定した範囲を超えた場合
        {
            isRushing = false;
            _pauseTimer = _pauseDuration; // 一時停止タイマーを設定
        }

        // プレイヤーの方向に向かって移動
        if (isRushing)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.position += direction * _rushSpeed * Time.deltaTime;
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

            print("突進停止（プレイヤーと衝突）");
        }
    }
}
