using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings; // UnityEditor の PlayerSettings を使用しているが、ゲーム実行時には不要なため、削除を検討

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField, Header("カメラのズーム速度")]
    private float _zoomSpeed;
    [SerializeField, Header("追従する速度")]
    private float _followSpeed;
    [SerializeField, Header("レイキャストの長さ")]
    float _rayLength = 0.5f;

    [SerializeField]
    GameObject _player;
    PlayerController_2D _playerController2D;

    Vector3 _offset;//初期のプレイヤーとカメラの差
    Vector3 _zoomPos; // ズーム時の目標位置
    Vector3 _pos; // カメラの現在の位置（Lerp用）

    float _firstDis; // カメラとプレイヤーの初期距離
    float _dis; // カメラとプレイヤーの現在の距離
    float _zoomValue; // ズーム時に変更するZ軸の値
    float _zoomObjSpeed = 0.05f; // オブジェクトズーム時の速度
    float _zoomObjDistance;

    private bool _isZoom = false; // 現在ズーム中かどうかのフラグ
    private bool _isFollowX = true; // X軸方向への追従を行うかどうかのフラグ
    private bool _isFollowY = true; // Y軸方向への追従を行うかどうかのフラグ
    bool _isCameraShake = false; // カメラシェイク中かどうかのフラグ
    bool _isObjZoom = false; // オブジェクトズーム中かどうかのフラグ

    // Start is called before the first frame update
    void Start()
    {
        _playerController2D = _player.GetComponent<PlayerController_2D>();
        _offset = _player.transform.position - this.transform.position;
        _pos = transform.position; // カメラの初期位置を保存
        _firstDis = Vector3.Distance(_player.transform.position, transform.position);
    }

    private void Update()
    {
        ObjDetection(); // 障害物検出を毎フレーム実行
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_player != null)
        {
            if (_isObjZoom)
            {
                ZoomObjUpdate(); // オブジェクトズーム時のカメラ移動処理
            }
            else
            {
                FollowUpdate(); // 通常のカメラ追従処理
                ZoomUpdate(); // 通常のカメラズーム処理
            }
        }
    }

    // カメラをプレイヤーに追従させる処理
    void FollowUpdate()
    {
        // Y軸方向への追従が有効な場合、カメラのY座標をプレイヤーに合わせてLerpで移動
        if (_isFollowY) _pos.y = Mathf.Lerp(_pos.y, _player.transform.position.y - _offset.y, _followSpeed);

        // X軸方向への追従が有効な場合、カメラのX座標をプレイヤーに合わせてLerpで移動
        if (_isFollowX) _pos.x = Mathf.Lerp(_pos.x, _player.transform.position.x - _offset.x, _followSpeed);
        //_posの位置に移動
        transform.position = _pos; // カメラを計算した位置に移動
    }

    // カメラのズーム処理
    void ZoomUpdate()
    {
        // ズーム中の場合、カメラのZ座標を目標値までLerpで移動
        if (_isZoom)
        {
            _pos.z = Mathf.Lerp(_pos.z, _player.transform.position.z - _offset.z + _zoomValue, _zoomSpeed);
        }
        // ズーム中でない場合、カメラのZ座標を初期値までLerpで移動
        else
        {
            _pos.z = Mathf.Lerp(_pos.z, _player.transform.position.z - _offset.z, _zoomSpeed);
        }
    }

    // オブジェクトズーム時のカメラ移動処理
    void ZoomObjUpdate()
    {
        // ズーム中の場合
        if (_isZoom)
        {
            //_zoomObjPosに近づく（_pos.zのみに_zoomValueを足したいので個別）
            _pos = Vector3.Lerp(_pos, _zoomPos - _offset, _zoomObjSpeed);
            //_posの位置に移動
            transform.position = _pos; // 計算した位置にカメラを移動
        }
        if (!_isZoom)
        {
            _pos = Vector3.Lerp(_pos, _player.transform.position - _offset, _zoomObjSpeed); // カメラをプレイヤーの位置に戻す
            //_posの位置に移動
            transform.position = _pos; // 計算した位置にカメラを移動

            // 目標地点に到達したかチェック
            if (Vector3.Distance(transform.position, _player.transform.position - _offset) < 0.01f) // しきい値を設定
            {
                ObjZommFinish(); // ズーム終了処理を即時呼び出し
            }
        }
    }

    // オブジェクトズーム終了処理
    void ObjZommFinish()
    {
        //プレイヤーが動けるように
        _playerController2D.InputActionEnable(); // プレイヤーの入力を有効にする
        _isObjZoom = false; // オブジェクトズームフラグを解除
    }

    // カメラをズームインさせる
    public void Zoom(float value)
    {
        if (_isZoom) return; // ズーム中の場合は処理を中断
        _zoomValue = value; // ズーム値を設定
        //ズーム中に変更
        _isZoom = true; // ズームフラグを立てる
    }

    // 特定のオブジェクトに対してズームインする
    public void ZoomObj(float value, Vector3 pos, float _speed)
    {
        _zoomValue = value; // ズーム値を設定
        _zoomPos = pos; // ズーム時の目標位置を設定
        _zoomPos.z += _zoomValue;//z軸のズーム量を加算
        _zoomObjSpeed = _speed; // オブジェクトズーム時の速度を設定
        //ズーム中に変更
        _isZoom = true; // ズームフラグを立てる
        _isObjZoom = true; // オブジェクトズームフラグを立てる
    }

    // カメラをズームアウトさせる
    public void ZoomOut()
    {
        if (!_isZoom) return; // ズーム中でない場合は処理を中断

        //ズーム中ではない状態に変更
        _isZoom = false; // ズームフラグを解除
    }

    // カメラ揺れ（デフォルト値 : 0.25f, 0.1f）
    IEnumerator CameraShake(float duration, float magnitude)
    {
        //画面揺れ中か判定
        if (_isCameraShake) yield break; // カメラシェイク中の場合は処理を中断

        var _beforePos = transform.localPosition; // シェイク前のカメラ位置を保存

        var _beforeRot = transform.localRotation; // シェイク前のカメラ回転を保存

        var _elapsed = 0f; // 経過時間を初期化

        _isCameraShake = true; // カメラシェイクフラグを立てる

        // シェイク時間の間
        while (_elapsed < duration)
        {
            // ランダムな値を設定してカメラを揺らす
            var x = _beforePos.x + Random.Range(-1f, 1f) * magnitude;
            var y = _beforePos.y + Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, _beforePos.z); // カメラの位置を更新
            _elapsed += Time.deltaTime; // 経過時間を加算

            yield return null;
        }

        //画面揺れ前の位置に戻す
        transform.localPosition = _beforePos; // カメラを元の位置に戻す
        transform.localRotation = _beforeRot; // カメラを元の回転に戻す
        _isCameraShake = false; // カメラシェイクフラグを解除
    }

    //止まっている時にのみ使用（カットシーン的な）
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(CameraShake(duration, magnitude)); // カメラシェイクコルーチンを開始
    }

    //カメラの進行方向にオブジェクトがあったら進まない
    void ObjDetection()
    {
        Vector3 directionX = new Vector3(_player.transform.position.x - _offset.x - transform.position.x, 0, 0).normalized; // X軸方向へのレイキャストの方向
        Vector3 directionY = new Vector3(0, _player.transform.position.y - _offset.y - transform.position.y, 0).normalized; // Y軸方向へのレイキャストの方向

        Ray rayX = new Ray(transform.position, directionX); // X軸方向へのレイを生成
        RaycastHit hitX;
        if (Physics.Raycast(rayX, out hitX, _rayLength)) // レイが障害物に当たった場合
        {
            if (hitX.collider.gameObject.CompareTag("Water"))
            {
                return;
            }
            _isFollowX = false; // X軸方向への追従を禁止
        }
        else
        {
            _isFollowX = true; // X軸方向への追従を許可
        }

        Ray rayY = new Ray(transform.position, directionY); // Y軸方向へのレイを生成
        Debug.DrawRay(transform.position, directionY, Color.red, 1.0f);
        RaycastHit hitY;
        if (Physics.Raycast(rayY, out hitY, _rayLength)) // レイが障害物に当たった場合
        {
            if (hitY.collider.gameObject.CompareTag("Water"))
            {
                return;
            }
            _isFollowY = false; // Y軸方向への追従を禁止
            Debug.Log(hitY.collider.gameObject.name);
        }
        else
        {
            _isFollowY = true; // Y軸方向への追従を許可
        }
    }
}