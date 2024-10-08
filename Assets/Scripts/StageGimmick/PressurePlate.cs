using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PressurePlate : MonoBehaviour, ISwitch
{
    [SerializeField,Header("押している間のみ反応するかどうか")]
    bool _isLongPress = false;
    bool _isPush = false;
    bool ISwitch.OnOffCheck()
    {
        return _isPush;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            _isPush = true;
            
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && _isLongPress)
        {
            _isPush = false;
        }
    }
}
