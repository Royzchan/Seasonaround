using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField, Header("弾の速度")]
    private float _bulletSpeed = 10f;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.AddForce(this.gameObject.transform.forward * _bulletSpeed, ForceMode.Impulse);
    }
    // Update is called once per frame
    void Update()
    {
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
