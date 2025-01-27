using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageWall : MonoBehaviour
{
    [SerializeField, Header("É_ÉÅÅ[ÉWó ")]
    int _damage;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.Damage(_damage);
        }
    }
}
