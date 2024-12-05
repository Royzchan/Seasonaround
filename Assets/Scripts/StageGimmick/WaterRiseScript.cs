using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRiseScript : MonoBehaviour
{
    [SerializeField, Header("���͑��u")]
    GameObject _input;
    [SerializeField, Header("����̃g�����X�t�H�[��")]
    Transform _waterPivot;
    [SerializeField, Header("���ʏ㏸��")]
    float _riseValue;
    [SerializeField, Header("�����ɂ����鎞��")]
    float _riseTime = 3f;
    bool _isRising = false;
    // Start is called before the first frame update
    void Start()
    {
        if (_input != null)
        {
            if (!_input.TryGetComponent<ISwitch>(out ISwitch sw))
            {
                Debug.LogError(name + "��_input�ɕs���Ȓl�����͂���Ă��܂�");
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
