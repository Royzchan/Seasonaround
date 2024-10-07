using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField, Header("’e‚Ì‘¬“x")]
    private float _bulletSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        // ’e‚ğ‘O•û‚ÉˆÚ“®‚³‚¹‚é
        transform.Translate(Vector3.forward * _bulletSpeed * Time.deltaTime);

        Destroy(this.gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // •Ç‚âƒvƒŒƒCƒ„[‚É“–‚½‚Á‚½‚ç’e‚ğÁ‚·
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);  // ’e‚ğíœ
        }
    }
}
