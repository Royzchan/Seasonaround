using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObjectScript : MonoBehaviour
{
    [SerializeField,Header("スイッチ")]
    GameObject _input;
    ISwitch _switch;
    // Start is called before the first frame update
    void Start()
    {
        if (_input != null)
        {

            if (_input.TryGetComponent<ISwitch>(out ISwitch sw))
            {
                _switch = sw;
            }
            else
            {
                Debug.LogError(name + "に不正なスイッチが入っています");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.SetActive(!_switch.OnOffCheck());
    }
}
