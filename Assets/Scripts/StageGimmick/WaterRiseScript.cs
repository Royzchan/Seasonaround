using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRiseScript : MonoBehaviour
{
    [SerializeField, Header("入力装置")]
    GameObject _input;
    [SerializeField, Header("水低のトランスフォーム")]
    Transform _waterPivot;
    [SerializeField, Header("水面上昇量")]
    float _riseValue;
    [SerializeField, Header("満潮にかかる時間")]
    float _riseTime = 3f;
    bool _isRising = false;
    // Start is called before the first frame update
    void Start()
    {
        if (_input != null)
        {
            if (!_input.TryGetComponent<ISwitch>(out ISwitch sw))
            {
                Debug.LogError(name + "の_inputに不正な値が入力されています");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_input.GetComponent<ISwitch>().OnOffCheck())
        {
            if (!_isRising)
            {
                RiseWater();
            }
        }
    }

    void RiseWater()
    {
        _isRising = true;
        _waterPivot.DOScaleY(_riseValue,_riseTime);
    }
}
