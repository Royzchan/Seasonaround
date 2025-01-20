using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ステージの復活地点にしたい場所にこのスクリプトを持った<br/>
/// オブジェクトをTriggerのColliderをつけて配置してください
/// </summary>
public class RestartPosition : MonoBehaviour
{
    Vector3 _pos;
    RestartManager _rm;
    int _index = -1;
    public int Index {set { _index = value; } }
    private void Awake()
    {
        _pos = transform.position;
    }
    private void Start()
    {
        _rm = FindAnyObjectByType<RestartManager>();
        if(_rm == null)
        {
            _rm = gameObject.AddComponent<RestartManager>();
        }
    }

    public Vector3 GetPosition()
    {
        return _pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _rm.ChangePositionNum(_index);
        }
    }
}
