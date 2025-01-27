using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkObj : MonoBehaviour
{
    [SerializeField,Header("Y軸の回転角度")]
    float _angleY;
    [SerializeField, Header("回転速度")]
    float _rotateSpeed;

    bool _canTurn = false; // 振り向きが可能かどうか

    Animator _talkObjAnimator; // このオブジェクトのアニメーターへの参照

    // Start is called before the first frame update
    void Start()
    {
        _talkObjAnimator = GetComponent<Animator>(); // このオブジェクトのアニメーターを取得
        if (_talkObjAnimator == null) _talkObjAnimator = this.transform.GetChild(0).GetComponent<Animator>(); // アニメーターがなければ、最初の子オブジェクトから取得
    }

    // Update is called once per frame
    void Update()
    {
        // _canTurnがtrue（振り向き可能）の場合
        if (_canTurn)
        {
            // オブジェクトを目標角度に向かって回転
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, _angleY, 0), _rotateSpeed);
        }
    }

    public void Turn()
    {
        _canTurn = true; // 振り向きを可能にする
    }

    public void IsShiver()
    {
        _talkObjAnimator.SetBool("isShiver", true);
    }

    public void IsJump()
    {
        _talkObjAnimator.SetBool("isJump", true);
    }

    public void IsSlant()
    {
        _talkObjAnimator.SetBool("isSlant", true);
    }

    public void IsLargeIdle()
    {
        _talkObjAnimator.SetBool("isLargeIdle", true);
    }

    public void IsLookOver()
    {
        _talkObjAnimator.SetBool("isLookOver", true);
    }

    public void IsQuestion()
    {
        _talkObjAnimator.SetTrigger("isQuestion");
    }

    public void IsSmallJump()
    {
        _talkObjAnimator.SetTrigger("isSmallJump");
    }

    // アニメーターのすべてのBoolをfalseにする（アイドル状態に戻す）
    public void IsIdle()
    {
        _talkObjAnimator.SetBool("isShiver", false);
        _talkObjAnimator.SetBool("isJump", false);
        _talkObjAnimator.SetBool("isSlant", false);
        _talkObjAnimator.SetBool("isLargeIdle", false);
        _talkObjAnimator.SetBool("isLookOver", false);
        _talkObjAnimator.SetBool("isStretch", false);
    }
}