using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class RestartManager : MonoBehaviour
{
    static int _restartNum;
    [SerializeField,Header("復活地点の位置(順番に入れてね)")]
    RestartPosition[] _positions;

    private void Start()
    {
       for(int i = 0;i < _positions.Length;++i)
        {
            _positions[i].Index = i;
        }
    }
    public void ResetRestartPos()
    {
        _restartNum = 0;
    }

    public Vector3 GetRestartPosition()
    {
        if (_positions.Length < _restartNum - 1)
        {
            Debug.LogError("_positionsの大きさを超えています");
        }
        Debug.Log(_positions[_restartNum].GetPosition());
        return _positions[_restartNum].GetPosition();

    }
    /// <summary>
    /// Restart位置を変更します<br/>
    /// ただし、indexが現在の_restartNumより小さければ無視します
    /// </summary>
    public void ChangePositionNum(int index)
    {
        if (_restartNum >= index) return;
        _restartNum = index;
        
    }
}
