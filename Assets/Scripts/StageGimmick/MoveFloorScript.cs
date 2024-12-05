using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveFloorScript : MonoBehaviour
{
    //扉の移動方向
    enum Direction
    {
        up,
        down,
        right,
        left
    }
    Dictionary<Direction, Vector3> dirVec = new Dictionary<Direction, Vector3>();
    [SerializeField, Header("入力装置")]
    GameObject _input;
    [SerializeField, Header("移動量")]
    float _moveValue = 50f;
    [SerializeField, Header("動く速度")]
    float _moveSpeed = 5f;
    [SerializeField, Header("反転するまでの時間")]
    float _relativeTime = 3f;
    [SerializeField, Header("移動方向")]
    Direction _moveDir;
    Vector3 _startPos;
    PlayerController_2D _player;
    bool _isMoving = false;
    bool _isRelativeStop = false;
    bool _isRelative = false;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindAnyObjectByType<PlayerController_2D>();
        if(_input != null)
        {
            if (!_input.TryGetComponent<ISwitch>(out ISwitch sw))
            {
                Debug.LogError(name + "の_inputに不正な値が入力されています");
            }
        }
        else
        {
            _isMoving = true;
        }
        _startPos = transform.position;
        dirVec.Add(Direction.up, new Vector3(0, 1, 0));
        dirVec.Add(Direction.down, new Vector3(0, -1, 0));
        dirVec.Add(Direction.right, new Vector3(1, 0, 0));
        dirVec.Add(Direction.left, new Vector3(-1, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (_input != null)
        {
            _isMoving = _input.GetComponent<ISwitch>().OnOffCheck();
        }

            //入力がONだった場合
        if (_isMoving &&!_isRelativeStop)
        {
            if(!_isRelative)
            {
                //移動速度に方向ベクトルを掛けて足す
                transform.position += dirVec[_moveDir] * _moveSpeed * Time.deltaTime;
                //位置補正
                transform.position = ClampPos(_startPos, _startPos + dirVec[_moveDir] * _moveValue);
                if(transform.position == _startPos + dirVec[_moveDir] * _moveValue)
                {
                    _isRelative = true;
                    StartCoroutine(SetRelativeStop());
                }
            }
            else
            {
                //移動速度に方向ベクトルを掛けて引く
                transform.position -= dirVec[_moveDir] * _moveSpeed * Time.deltaTime;
                //位置補正
                transform.position = ClampPos(_startPos, _startPos + dirVec[_moveDir] * _moveValue);
                if (transform.position == _startPos)
                {
                    _isRelative = false;
                    StartCoroutine(SetRelativeStop());
                }
            }
           
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            _player.transform.parent = transform;

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            _player.transform.parent = null;
            
            
        }
    }
    IEnumerator SetRelativeStop()
    {
        _isRelativeStop = true;
        yield return new WaitForSeconds(_relativeTime);
        _isRelativeStop = false;
    }
    //くそ雑なのでもしかしたら修正するかも
    Vector3 ClampPos(Vector3 value1, Vector3 value2)
    {
        Vector3 newPos = new();
        if (value1.x < value2.x)
        {
            newPos.x = Mathf.Clamp(transform.position.x, value1.x, value2.x);
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
