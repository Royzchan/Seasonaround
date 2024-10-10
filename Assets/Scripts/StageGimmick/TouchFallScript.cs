using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using UnityEngine;

public class TouchFallScript : MonoBehaviour
{
    [SerializeField,Header("������܂ł̎���")]
    float _startTime = 3f;
    [SerializeField, Header("�k�����")]
    float _shakePower = 0.2f;
    float _timer;
    //�ӂꂽ��
    bool _isTouch = false;
    //�J�n�ʒu(�����ɂ�ۂ���)
    Vector3 _startPos;
    Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position;
        _rb = GetComponent<Rigidbody>();
        //�Œ�(�����Ă��邩�̔���͂��̕ϐ���p����)
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
        //���������Ă���Œ��ɂԂ�����������A���Z�b�g���ď����ʒu��
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
