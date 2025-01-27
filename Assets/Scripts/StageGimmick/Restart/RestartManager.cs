using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class RestartManager : MonoBehaviour
{
    static int _restartNum;
    [SerializeField,Header("�����n�_�̈ʒu(���Ԃɓ���Ă�)")]
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
            Debug.LogError("_positions�̑傫���𒴂��Ă��܂�");
        }
        Debug.Log(_positions[_restartNum].GetPosition());
        return _positions[_restartNum].GetPosition();

    }
    /// <summary>
    /// Restart�ʒu��ύX���܂�<br/>
    /// �������Aindex�����݂�_restartNum��菬������Ζ������܂�
    /// </summary>
    public void ChangePositionNum(int index)
    {
        if (_restartNum >= index) return;
        _restartNum = index;
        
    }
}
