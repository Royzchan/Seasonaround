using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSwitch : MonoBehaviour
{
    [SerializeField, Header("アクティブにしたいオブジェクト")]
    public GameObject _targetObject;
    [SerializeField, Header("スイッチの初期位置")]
    private Vector3 _initialPosition;
    [SerializeField, Header("押し込む距離")]
    public float _pushDistance = 0.1f;
    [SerializeField, Header("押し込む速さ")]
    public float _pushSpeed = 0.2f;
    [SerializeField, Header("スイッチが押されたかどうか")]
    private bool isPushed = false;

    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = transform.position;  // スイッチの初期位置を保存
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isPushed && collision.gameObject.CompareTag("Player"))  // プレイヤーがスイッチに触れたら
        {
            isPushed = true;
            StartCoroutine(PushSwitch());  // スイッチを押すアニメーションを開始
        }
    }

    IEnumerator PushSwitch()
    {
        Vector3 targetPosition = _initialPosition - new Vector3(0, _pushDistance, 0);  // 押し込む位置
        float elapsedTime = 0f;

        // 押し込むアニメーション
        while (elapsedTime < _pushSpeed)
        {
            transform.position = Vector3.Lerp(_initialPosition, targetPosition, elapsedTime / _pushSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;  // 最終的に押し込んだ位置に設定

        // オブジェクトをアクティブにする
        _targetObject.SetActive(true);
    }
}
