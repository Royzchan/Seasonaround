using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SlopeRotationScript : MonoBehaviour
{
    [SerializeField,Header("入力数値")]
    GameObject _input;
    ISwitch _switch;
    [SerializeField,Header("回転速度")]
    float _rotateTime = 0.3f;
    [SerializeField,Header("回転後の数値")]
    Vector3 _rotateAngle = Vector3.zero;
    bool _isRotation = false;
    // Start is called before the first frame update
    void Start()
    {
        if(_input != null)
        {
            if (!_input.TryGetComponent<ISwitch>(out _switch))
            {
                Debug.LogError(gameObject.name + "に不正な入力がついています");
            }
        }
        else
        {
            MoveRotation();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_switch != null)
        {
            if (_switch.OnOffCheck() && !_isRotation)
            {
                MoveRotation();
            }
        }
    }

    void MoveRotation()
    {
        _isRotation = true;
        transform.DORotate(_rotateAngle,_rotateTime);
    }
}
