 using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    //���̈ړ�����
    enum Direction
    {
        up,
        down,
        right,
        left,
        front,
        back
    }
    Dictionary<Direction,Vector3> dirVec = new Dictionary<Direction,Vector3>();
    [SerializeField,Header("���͑��u")]
    GameObject _input;
    [SerializeField, Header("�ړ���")]
    float _moveValue = 50f; 
    [SerializeField, Header("�J�����x")]
    float _openSpeed = 5f;
    [SerializeField, Header("�܂鑬�x")]
    float _closeSpeed = 5f;
    [SerializeField, Header("�J������")]
    Direction _openDir;
    Vector3 _startPos;
    // Start is called before the first frame update
    void Start()
    {

        if (!_input.TryGetComponent<ISwitch>(out ISwitch sw))
        {
            Debug.LogError(name + "��_input�ɕs���Ȓl�����͂���Ă��܂�");
        }
        _startPos = transform.position;
        dirVec.Add(Direction.up, new Vector3(0, 1, 0));
        dirVec.Add(Direction.down, new Vector3(0, -1, 0));
        dirVec.Add(Direction.right, new Vector3(1, 0, 0));
        dirVec.Add(Direction.left, new Vector3(-1, 0, 0));
        dirVec.Add(Direction.front, new Vector3(0, 0, 1));
        dirVec.Add(Direction.back, new Vector3(0, 0, -1));
    }

    // Update is called once per frame
    void Update()
    {
       
        //���͂�ON�������ꍇ
        if (_input.GetComponent<ISwitch>().OnOffCheck())
        {  
            //�ړ����x�ɕ����x�N�g�����|���đ���
            transform.position += dirVec[_openDir] * _openSpeed * Time.deltaTime;
            //�ʒu�␳
           transform.position = ClampPos(_startPos, _startPos + dirVec[_openDir] * _moveValue);
        }
        else
        {
            //�ړ����x�ɕ����x�N�g�����|���Ĉ���
            transform.position -= dirVec[_openDir] * _closeSpeed * Time.deltaTime;
            //�ʒu�␳
            transform.position = ClampPos(_startPos, _startPos + dirVec[_openDir] * _moveValue);
        }
       
    }
    //�����G�Ȃ̂ł�����������C�����邩��
    Vector3 ClampPos(Vector3 value1,Vector3 value2)
    {
        Vector3 newPos = new();
        if(value1.x < value2.x)
        {
            newPos.x = Mathf.Clamp(transform.position.x,value1.x,value2.x);
        }
        else
        {
            newPos.x = Mathf.Clamp(transform.position.x, value2.x, value1.x);
        }
        if (value1.y < value2.y)
        {
            newPos.y = Mathf.Clamp(transform.position.y, value1.y, value2.y);
        }
        else
        {
            newPos.y = Mathf.Clamp(transform.position.y, value2.y, value1.y);
        }
        if (value1.z < value2.z)
        {
            newPos.z = Mathf.Clamp(transform.position.z, value1.z, value2.z);
        }
        else
        {
            newPos.z = Mathf.Clamp(transform.position.z, value2.z, value1.z);
        }
        return newPos;
    }
}
