using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField, Header("ズーム時のスピード")]
    private float _zoomSpeed;
    [SerializeField, Header("追従のスピード")]
    private float _followSpeed;

    [SerializeField]
    GameObject _player;//プレイヤー
    PlayerController_2D _playerController2D;

    Vector3 _offset;//初期のプレイヤーとカメラの差
    Vector3 _zoomOffset;
    Vector3 _pos;

    float _firstDis;//最初の距離
    float _dis;//現在の距離

    //ズーム中かどうか
    private bool _isZoom = false;

    bool _isCameraShake = false;

    enum State
    {
        Follow,
        Zoom
    }
    State _state = State.Follow;

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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_player != null)
        {
            if (_state == State.Follow)
            {
                FollowUpdate();
            }
            else if (_state == State.Zoom)
            {
                ZoomUpdate();
            }
        }
    }

    void FollowUpdate()
    {
        if (_isZoom)
        {
            // カメラの追従
           
            _pos.y = _player.transform.position.y - _offset.y;
            
            _pos.x = _player.transform.position.x - _zoomOffset.x;
        }
        else
        {
            // カメラの追従
            
            _pos.y = _player.transform.position.y - _offset.y;

            _pos.x = _player.transform.position.x - _offset.x;
        }
        //_posの位置に移動
        transform.position = Vector3.Lerp(transform.position, _pos, _followSpeed);
    }

    void ZoomUpdate()
    {
        if (_isZoom)
        {
            _pos.x = _player.transform.position.x;
            if (_playerController2D.ISFly) _pos.y = _player.transform.position.y - _offset.y;
        }
        else
        {
            _pos.x = _player.transform.position.x - _offset.x;
            if (_playerController2D.ISFly) _pos.y = _player.transform.position.y - _offset.y;
        }

        //_posの位置に移動
        transform.position = Vector3.Lerp(transform.position, _pos, _zoomSpeed);
        if (transform.position == _pos)
        {
            _state = State.Follow;
            _zoomOffset = transform.position - transform.position;
        }
    }

    public void Zoom(float value)
    {
        if (_isZoom) return;

        _state = State.Zoom;
        _pos.z = _player.transform.position.z - _offset.z + value;
        //ズーム中に変更
        _isZoom = true;
    }

    public void ZoomOut()
    {
        if (!_isZoom) return;

        _pos.z = _player.transform.position.z - _offset.z;

        //ズーム中ではない状態に変更
        _isZoom = false;
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
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(CameraShake(duration, magnitude));
    }
}
