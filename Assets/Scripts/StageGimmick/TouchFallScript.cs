using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using UnityEngine;

public class TouchFallScript : MonoBehaviour
{
    [SerializeField,Header("落ちるまでの時間")]
    float _startTime = 3f;
    [SerializeField, Header("震える力")]
    float _shakePower = 0.2f;
    float _timer;
    //ふれたか
    bool _isTouch = false;
    //開始位置(ここにりぽっぷ)
    Vector3 _startPos;
    Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position;
        _rb = GetComponent<Rigidbody>();
        //固定(落ちているかの判定はこの変数を用いる)
        _rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(!_isTouch)
            {
                _isTouch = true;
                StartCoroutine(ShakeFloar());
            }
        }
        //もし落ちている最中にぶつかったら消去、リセットして初期位置へ
        else if(!_rb.isKinematic)
        {
            FloarReset();
            gameObject.SetActive(false);
        }
        
    }
    IEnumerator ShakeFloar()
    {
        transform.DOPunchPosition(new Vector3(_shakePower, 0, 0), _startTime);
        yield return new WaitForSeconds(_startTime);
        _rb.isKinematic = false;
    }
    void FloarReset()
    {
        _rb.isKinematic = true;
        _timer = 0;
        _isTouch = false;
        transform.position = _startPos;
    }
}
