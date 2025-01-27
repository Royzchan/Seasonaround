using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �X�e�[�W�̕����n�_�ɂ������ꏊ�ɂ��̃X�N���v�g��������<br/>
/// �I�u�W�F�N�g��Trigger��Collider�����Ĕz�u���Ă�������
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
