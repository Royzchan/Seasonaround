using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallScript : MonoBehaviour
{
    [SerializeField,Header("“ü—Í‘•’u")]
    GameObject _input;
    Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        if(!_input.TryGetComponent<ISwitch>(out ISwitch sw))
        {
            Debug.LogError(name + "‚Ì_input‚É•s³‚È’l‚ª“ü—Í‚³‚ê‚Ä‚¢‚Ü‚·");
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
