using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObjectScript : MonoBehaviour
{
    [SerializeField,Header("�X�C�b�`")]
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
                Debug.LogError(name + "�ɕs���ȃX�C�b�`�������Ă��܂�");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.SetActive(!_switch.OnOffCheck());
    }
}
