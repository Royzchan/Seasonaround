using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField, Header("ズーム時のスピード")]
    private float _zoomSpeed;
    [SerializeField, Header("追従のスピード")]
    private float _followSpeed;
    [SerializeField,Header("Rayの長さ")]
    float _rayLength = 0.5f;

    [SerializeField]
    GameObject _player;//プレイヤー
    PlayerController_2D _playerController2D;

    Vector3 _offset;//初期のプレイヤーとカメラの差
    Vector3 _zoomOffset;
    Vector3 _pos;

    float _firstDis;//最初の距離
    float _dis;//現在の距離
    float _zoomValue;
    float _zoomObjTime;
    Vector3 _zoomObjPos;

    //ズーム中かどうか
    private bool _isZoom = false;
    private bool _isFollowX = true;
    private bool _isFollowY = true;
    bool _isCameraShake = false;
    bool _isObjZoom = false;

    private Vector3 _previousPosition; // 前のフレームの位置

    // Start is called before the first frame update
    void Start()
    {
        _playerController2D = _player.GetComponent<PlayerController_2D>();
        _offset = _player.transform.position - this.transform.position;
        _pos = transform.position;
        _firstDis = Vector3.Distance(_player.transform.position, transform.position);
    }

    private void Update()
    {
        //デバッグ
        if (Input.GetKeyDown(KeyCode.N))
        {
            Zoom(2);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            ZoomOut();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Shake(5, 0.1f);
        }

        ObjDetection();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_player != null)
        {
            if (_isObjZoom)
            {
                ZoomObjUpdate();
            }
            else
            {
                FollowUpdate();
                ZoomUpdate();
            }
        }
    }

    void FollowUpdate()
    {
        if (_isFollowY) _pos.y = Mathf.Lerp(_pos.y, _player.transform.position.y - _offset.y, _followSpeed);

        if (_isFollowX) _pos.x = Mathf.Lerp(_pos.x, _player.transform.position.x - _offset.x, _followSpeed);
        //_posの位置に移動
        transform.position = _pos;
    }

    void ZoomUpdate()
    {
        if (_isZoom)
        {
            _pos.z = Mathf.Lerp(_pos.z, _player.transform.position.z - _offset.z + _zoomValue, _zoomSpeed);
        }
        else
        {
            _pos.z = Mathf.Lerp(_pos.z, _player.transform.position.z - _offset.z, _zoomSpeed);
        }
    }

    void ZoomObjUpdate()
    {
        _pos = Vector3.Lerp(_pos, _zoomObjPos, _zoomSpeed);
        _zoomObjTime -= Time.deltaTime;
        if(_zoomObjTime <= 0)
        {
            ZoomOut();
        }
    }

    public void Zoom(float value)
    {
        if (_isZoom) return;
        _zoomValue = value;
        //ズーム中に変更
        _isZoom = true;
    }

    public void ZoomObj(float value, Vector3 pos, float time)
    {
        if (_isZoom) return;
        _zoomObjPos = pos;
        _zoomObjPos.z = _player.transform.position.z - _offset.z + _zoomValue;
        _zoomObjTime = time;
        //ズーム中に変更
        _isZoom = true;
        _isObjZoom = true;
    }

    public void ZoomOut()
    {
        if (!_isZoom) return;

        //ズーム中ではない状態に変更
        _isZoom = false;
        _isObjZoom = false;
    }

    //カメラ揺れ（デフォルト値 : 0.25f, 0.1f）
    IEnumerator CameraShake(float duration, float magnitude)
    {
        //画面揺れ中か判定
        if (_isCameraShake) yield break;

        var _beforePos = transform.localPosition;

        var _beforeRot = transform.localRotation;

        var _elapsed = 0f;

        _isCameraShake = true;

        while (_elapsed < duration)
        {
            var x = _beforePos.x + Random.Range(-1f, 1f) * magnitude;
            var y = _beforePos.y + Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, _beforePos.z);
            _elapsed += Time.deltaTime;

            yield return null;
        }

        //画面揺れ前の位置に戻す
        transform.localPosition = _beforePos;
        transform.localRotation = _beforeRot;
        _isCameraShake = false;
    }

    //止まっている時にのみ使用（カットシーン的な）
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(CameraShake(duration, magnitude));
    }

    //カメラの進行方向にオブジェクトがあったら進まない
    void ObjDetection()
    {
        Vector3 directionX = new Vector3(_player.transform.position.x - _offset.x - transform.position.x,0, 0).normalized;
        Vector3 directionY = new Vector3(0, _player.transform.position.y - _offset.y - transform.position.y, 0).normalized;
        
        // 移動方向にRayを飛ばして障害物を検知
        //X
        Ray rayX = new Ray(transform.position, directionX);
        // 障害物がなければ移動
        if (!Physics.Raycast(rayX, _rayLength))
        {
            _isFollowX = true;
        }
        else
        {
            _isFollowX = false;
        }
        //Y
        Ray rayY = new Ray(transform.position, directionY);
        // 障害物がなければ移動
        if (!Physics.Raycast(rayY, _rayLength))
        {
            _isFollowY = true;
        }
        else
        {
            _isFollowY = false;
        }
    }
}
