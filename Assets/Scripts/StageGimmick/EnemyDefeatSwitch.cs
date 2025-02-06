using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefeatSwitch : MonoBehaviour,ISwitch
{
    bool _isDefeatEnemy = false;
    [SerializeField,Header("敵のオブジェクト")]
    GameObject _enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    bool ISwitch.OnOffCheck()
    {
        return _isDefeatEnemy;
    }
    // Update is called once per frame
    void Update()
    {
        if (_enemy != null) return;
        _isDefeatEnemy = true;
        
    }
}
