using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TouchFallScriptを持った子がいないとバグるよ、とんでも脆弱性
public class TouchFallManager : MonoBehaviour
{
    
    [SerializeField, Header("復活までの時間")]
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
                Debug.LogError(name + "の_floarが不正です");
            }
        }
        else
        {
            Debug.LogError(name + "の_floarがnullです");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //floarがactiveしていない場合
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
