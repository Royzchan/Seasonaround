using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField,Header("ズーム時のスピード")]
    private float _zoomSpeed;
    [SerializeField,Header("追従のスピード")]
    private float _followSpeed;

    GameObject _player;//プレイヤー

    Vector3 _offset;//初期のプレイヤーとカメラの差
    Vector3 _pos;

    float _firstDis;//最初の距離
    float _dis;//現在の距離

    //ズーム中かどうか
    private bool isZoom = false;

    // 現在速度(SmoothDampの計算のために必要)
    private float _currentVelocity = 0;

    // 目標値に到達するまでのおおよその時間[s]
    [SerializeField] private float _smoothTime = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _offset = _player.transform.position - this.transform.position;
        _pos = transform.position;
        _firstDis = Vector3.Distance(_player.transform.position, transform.position);
    }

    private void Update()
    {
        //ズーム（仮）
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartCoroutine(Zoom(3));
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(ZoomOut());
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_player != null)
        {
            // カメラの追従
            _pos.x = _player.transform.position.x - _offset.x;

            //_posの位置に移動
            transform.position = Vector3.Lerp(transform.position, _pos, _followSpeed);
        }
    }

    public IEnumerator Zoom(float value)
    {
        if (isZoom) yield break; 

        //現在の距離を計算
        _dis = Vector3.Distance(_player.transform.position, transform.position);

        while (_firstDis - value < _dis)
        {
            //現在の距離を計算
            _dis = Vector3.Distance(_player.transform.position, transform.position);
            //ズーム
            _pos.z += _zoomSpeed * Time.deltaTime;
            yield return null;
        }

        //ズーム中に変更
        isZoom = true;
    }

    public IEnumerator ZoomOut()
    {
        if (!isZoom) yield break;

        //現在の距離を計算
        _dis = Vector3.Distance(_player.transform.position, transform.position);

        while (_firstDis > _dis)
        {
            //現在の距離を計算
            _dis = Vector3.Distance(_player.transform.position, transform.position);
            //ズームアウト
            _pos.z -= _zoomSpeed * Time.deltaTime;
            yield return null;
        }

        //ズーム中ではない状態に変更
        isZoom = false;
    }
}
