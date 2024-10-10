using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    GameObject player;

    [SerializeField, Header("プレイヤーとの距離")]
    private float _distance = 5f;

    [SerializeField, Header("弾のオブジェクト")]
    private GameObject bulletPrefab;

    [SerializeField, Header("弾の生成場所")]
    private Transform bulletPosition;

    [SerializeField, Header("生成間隔")]
    private float _repeatSpan = 3f;

    // 経過時間
    private float _timeElapsed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        _timeElapsed = 3;   //経過時間をリセット
    }

    // Update is called once per frame
    void Update()
    {
        _timeElapsed += Time.deltaTime;  // 時間をカウントする

        if (player != null)
        {
            // プレイヤーと敵の距離測定用
            float _dis = Vector3.Distance(this.transform.position, player.transform.position);

            // プレイヤーと敵の位置関係用
            float _compare = this.transform.position.x - player.transform.position.x;

            LeftorRight(_compare);
            Shoot(_dis);
        }
    }

    // 撃つ(弾のプレハブ生成用)
    void Shoot(float dis)
    {
        if (dis <= _distance)
        {
            if (_timeElapsed >= _repeatSpan)
            {
                // 弾の生成
                GameObject bullet = Instantiate(bulletPrefab, bulletPosition.position, bulletPosition.rotation);

                _timeElapsed = 0;
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
}
