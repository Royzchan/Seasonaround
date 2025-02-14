using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class WindHit : MonoBehaviour,ISwitch
{
    Rigidbody _rb;
    [SerializeField]
    float _windPower = 3.0f;
    bool _isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = (RigidbodyConstraints)10;
    }

    bool ISwitch.OnOffCheck()
    {
        return _isActive;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Wind"))
        {
            _rb.velocity = Vector3.up * _windPower;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            _isActive = true;
        }
    }
}
