using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressSwitch : MonoBehaviour
{
    [SerializeField, Header("入力装置")]
    GameObject _input;

    [SerializeField, Header("移動量")]
    float _moveValue = 1f;

    Vector3 _startPos;

    // Start is called before the first frame update
    void Start()
    {
        if (!_input.TryGetComponent<ISwitch>(out ISwitch sw))
        {
            Debug.LogError(name + "の_inputに不正な値が入力されています");
        }
        _startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.GetComponent<ISwitch>().OnOffCheck())
        {
            Input();
        }
    }

    void Input()
    {
        transform.position = new Vector3(transform.position.x, _startPos.y + _moveValue, transform.position.z);
    }
}
