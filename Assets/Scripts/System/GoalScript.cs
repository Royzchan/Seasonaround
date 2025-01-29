using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

public class GoalScript : MonoBehaviour
{
    PlayerController_2D _player;
    [SerializeField, Header("近寄る距離")]
    Vector3 _cameraDistance = new Vector3(0f,2f,-2f);
    [SerializeField, Header("カメラの移動時間")]
    float _cameraMoveTime = 1f;
    [SerializeField, Header("テキスト")]
    GameObject _clearText;
    [SerializeField, Header("テキストの拡大の時間")]
    float _textZoomTime = 1f;
    [SerializeField, Header("シーン遷移アニメーション")]
    RectTransform _transition;
    [SerializeField,Header("移行先のシーン")]
    string _sceneName;
    CameraFollow2D _cameraScript;
    [SerializeField, Header("PlayerPrefsのKey")]
    string _seasonKey;
    private void Start()
    {
        _player = FindAnyObjectByType<PlayerController_2D>();
        _cameraScript = FindAnyObjectByType<CameraFollow2D>();
        _clearText.transform.localScale = Vector3.zero;
        _transition.parent.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Goal()
    {
        //追従を切る
        _cameraScript.enabled = false;
        //プレイヤーの位置を取得
        Transform playerPos = _player.transform;
        //プレイヤーは操作不能
        _player.enabled = false;
        //着地を待つ
        yield return new WaitForSeconds(0.8f);
        //カメラをプレイヤーの位置に移動
        Camera.main.transform.DOMove(playerPos.position + _cameraDistance,_cameraMoveTime);
        yield return new WaitForSeconds(_cameraMoveTime + 0.5f);
        _clearText.gameObject.SetActive(true);
        //拡大
        _clearText.transform.DOScale(1f,_textZoomTime - _textZoomTime * 0.2f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(_textZoomTime);
        _transition.parent.gameObject.SetActive(true);
        //Maskの更新の関係で一瞬画面が真っ暗にちらつくため、透明にして一瞬待機後,戻す
        _transition.parent.GetComponent<Image>().color -= new Color(0,0,0,255); 
        yield return new WaitForEndOfFrame();
        _transition.parent.GetComponent<Image>().color += new Color(0, 0, 0, 255);
        //遷移用画像をTrueにして拡大
        //.gameObject.SetActive(true);
        _transition.DOSizeDelta(new Vector2(0,0), 1f);
        yield return new WaitForSeconds(1.5f);
        FadeManager.Instance.LoadScene(_sceneName,0.2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var restart = FindAnyObjectByType<RestartManager>();
            if (restart != null)
            {
                //終了時にリスポーン地点のリセット
                restart.ResetRestartPos();
            }
            PlayerPrefs.SetInt(_seasonKey,0);
            StartCoroutine(Goal());
        }
    }
}
