using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField, Header("’e‚Ì‘¬“x")]
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
        // •Ç‚âƒvƒŒƒCƒ„[‚É“–‚½‚Á‚½‚ç’e‚ğÁ‚·
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);  // ’e‚ğíœ
        }
    }
}
