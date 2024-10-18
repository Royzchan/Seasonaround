using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PressurePlate : MonoBehaviour, ISwitch
{
    [SerializeField,Header("押している間のみ反応するかどうか")]
    bool _isLongPress = false;
    bool _isPush = false;
    [SerializeField, Header("移動速度")]
    float _speed;
    float _timer;
    [SerializeField, Header("押し込んだ後の移動距離")]
    Vector3 _movePos;
    [SerializeField,Header("押し込めるオブジェクトのタグ")]
    string[] _collisionTags;
    Vector3 _startPos;

    private void Start()
    {
        _startPos = transform.position;
    }

    bool ISwitch.OnOffCheck()
    {
        return _isPush;
    }

    private void FixedUpdate()
    {
        if (_isPush)
        {
            if(_timer * _speed < 1f)
            {
                _timer += Time.fixedDeltaTime;
                transform.position = Vector3.Lerp(transform.position, _startPos + _movePos, _timer * _speed);
            }
        }
        else
        {
            if (_timer * _speed > 0f)
            {
                _timer -= Time.fixedDeltaTime;
                transform.position = Vector3.Lerp(transform.position, _startPos, _timer * _speed);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        foreach (var tag in _collisionTags)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                _isPush = true;
            }
        }


    }

    private void OnCollisionExit(Collision collision)
    {
        if (_isLongPress)
        foreach (var tag in _collisionTags)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                _isPush = false;
            }
        }
    }
}
