using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField, Header("弾の速度")]
    private float _bulletSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        // 弾を前方に移動させる
        transform.Translate(Vector3.forward * _bulletSpeed * Time.deltaTime);

        Destroy(this.gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 壁やプレイヤーに当たったら弾を消す
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);  // 弾を削除
        }
    }
}
