using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TouchFallScript���������q�����Ȃ��ƃo�O���A�Ƃ�ł��Ǝ㐫
public class TouchFallManager : MonoBehaviour
{
    
    [SerializeField, Header("�����܂ł̎���")]
    float _repopTime = 1f;
    GameObject _floar;
    float _timer;
    // Start is called before the first frame update
    void Start()
    {
        _floar = transform.GetChild(0).gameObject;
        if(_floar != null)
        {
            if(!_floar.TryGetComponent(out TouchFallScript t))
            {
                Debug.LogError(name + "��_floar���s���ł�");
            }
        }
        else
        {
            Debug.LogError(name + "��_floar��null�ł�");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //floar��active���Ă��Ȃ��ꍇ
        if (!_floar.activeSelf)
        {
            _timer += Time.deltaTime;
            if (_timer >= _repopTime)
            {
                _timer = 0;
                _floar.SetActive(true);
            }
        }
    }
}
