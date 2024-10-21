using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField, Header("ズーム時のスピード")]
    private float _zoomSpeed;
    [SerializeField, Header("追従のスピード")]
    private float _followSpeed;

    GameObject _player;//プレイヤー

    Vector3 _offset;//初期のプレイヤーとカメラの差
    Vector3 _pos;

    float _firstDis;//最初の距離
    float _dis;//現在の距離

    //ズーム中かどうか
    private bool _isZoom = false;

    bool _isCameraShake = false;

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
        if (_isZoom) yield break;

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
        _isZoom = true;
    }

    public IEnumerator ZoomOut()
    {
        if (!_isZoom) yield break;

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
