using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallController : MonoBehaviour
{
    private Rigidbody _rb;
    private bool _canSizeUp = true;
    private float _maxSize = 2.5f;
    [SerializeField]
    private float _sizeUpValue = 0.1f;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (_rb.velocity.x > 0)
        {
            SizeUp();
        }
    }

    private void SizeUp()
    {
        var sizeUpValue = new Vector3(_sizeUpValue, _sizeUpValue, _sizeUpValue);
        var size = transform.localScale.x;
        if (size < _maxSize)
        {
            this.transform.localScale += sizeUpValue * Time.deltaTime;
        }
        else
        {
            this.transform.localScale = new Vector3(_maxSize, _maxSize, _maxSize);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

    }
}
