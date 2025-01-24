using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkObj : MonoBehaviour
{
    [SerializeField,Header("Y���̉�]�p�x")]
    float _angleY;
    [SerializeField, Header("��]���x")]
    float _rotateSpeed;

    bool _canTurn = false; // �U��������\���ǂ���

    Animator _talkObjAnimator; // ���̃I�u�W�F�N�g�̃A�j���[�^�[�ւ̎Q��

    // Start is called before the first frame update
    void Start()
    {
        _talkObjAnimator = GetComponent<Animator>(); // ���̃I�u�W�F�N�g�̃A�j���[�^�[���擾
        if (_talkObjAnimator == null) _talkObjAnimator = this.transform.GetChild(0).GetComponent<Animator>(); // �A�j���[�^�[���Ȃ���΁A�ŏ��̎q�I�u�W�F�N�g����擾
    }

    // Update is called once per frame
    void Update()
    {
        // _canTurn��true�i�U������\�j�̏ꍇ
        if (_canTurn)
        {
            // �I�u�W�F�N�g��ڕW�p�x�Ɍ������ĉ�]
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, _angleY, 0), _rotateSpeed);
        }
    }

    public void Turn()
    {
        _canTurn = true; // �U��������\�ɂ���
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

    // �A�j���[�^�[�̂��ׂĂ�Bool��false�ɂ���i�A�C�h����Ԃɖ߂��j
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