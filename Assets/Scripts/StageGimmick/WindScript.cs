using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindScript : MonoBehaviour
{
    [SerializeField, Header("スイッチ")]
    GameObject _switchObj;
    [SerializeField, Header("風力")]
    float _windPower;
    ISwitch _switch;
    PlayerController_2D _pc;
    bool _isActive = true;
    Collider _collider;
    // Start is called before the first frame update
    void Start()
    {
        if (_switchObj != null)
        {
            
            if (_switchObj.TryGetComponent<ISwitch>(out ISwitch sw))
            {
                _switch = sw;
            }
            else
            {
                Debug.LogError(name + "に不正なスイッチが入っています");
            }
        }
        _collider = GetComponent<Collider>();
        _pc = FindAnyObjectByType<PlayerController_2D>();
    }

    private void OnValidate()
    {
        //GameObject p = GameObject.FindAnyObjectByType<PlayerController_2D>().transform.gameObject;
        //Debug.DrawRay(transform.position,transform.forward * 100,Color.red,Mathf.Infinity);
    }
    // Update is called once per frame
    void Update()
    { 
        if (_switch != null)
        {
            //スイッチの出力を_isActiveに
            _isActive = !_switch.OnOffCheck();
            _collider.enabled = _isActive;

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && _isActive)
        {
            
            _pc.SetWindPow(new Vector3(-(transform.forward * _windPower).x,-(transform.forward * _windPower).y,0));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && _isActive)
        {
            _pc.SetWindPow(new Vector3(-(transform.forward * _windPower).x,0,0));
        }
    }
}
