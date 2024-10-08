using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallScript : MonoBehaviour
{
    [SerializeField,Header("入力装置")]
    GameObject _input;
    Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        if(!_input.TryGetComponent<ISwitch>(out ISwitch sw))
        {
            Debug.LogError(name + "の_inputに不正な値が入力されています");
        }
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(_input != null)
        {
            if(_input.GetComponent<ISwitch>().OnOffCheck())
            {
                _rb.isKinematic = false;
            }
        }
    }
}
