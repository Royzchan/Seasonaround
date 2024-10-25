using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressSwitch : MonoBehaviour
{
    [SerializeField, Header("���͑��u")]
    GameObject _input;

    [SerializeField, Header("�ړ���")]
    float _moveValue = 1f;

    Vector3 _startPos;

    // Start is called before the first frame update
    void Start()
    {
        if (!_input.TryGetComponent<ISwitch>(out ISwitch sw))
        {
            Debug.LogError(name + "��_input�ɕs���Ȓl�����͂���Ă��܂�");
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
