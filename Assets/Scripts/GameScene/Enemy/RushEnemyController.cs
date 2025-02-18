using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushEnemyController : MonoBehaviour
{

    [SerializeField, Header("�ړ��X�s�[�h")]
    private float _speed;
    [SerializeField, Header("�ːi���n�߂鋗��")]
    private float _attackDistance;
    [SerializeField, Header("�ːi���s������"), Range(0.1f, 2.0f)]
    private float _attackTime = 2.0f;
    private float _rushTimeCount = 0f;

    private Rigidbody _rb; // ���W�b�h�{�f�B
    private GameObject _player; // �v���C���[
    private Animator _animator; // �A�j���[�^�[

    private bool _rushing = false; // �ːi���̃t���O
    private bool _checkPlayerDis = true; // �v���C���[�̋������m�F�����̃t���O
    private float _checkCoolTime = 5.0f; // �ːi��ēx�ːi����܂ł̃N�[���^�C��

    //�A�j���[�V�����֌W
    private string _attackStr = "Attack";


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null) Debug.LogError("RigidBody���o�^����Ă��܂���B");

        //�v���C���[�������ēo�^
        _player = FindAnyObjectByType<PlayerController_2D>().gameObject;
        if (_player == null) Debug.LogError("�v���C���[���o�^����Ă��܂���B");

        _animator = GetComponent<Animator>();
        if (_animator == null) Debug.LogError("�A�j���[�^�[���o�^����Ă��܂���B");
    }

    void Update()
    {
        if (_rushing)
        {
            _rb.velocity = Vector3.forward * _speed;
            _rushTimeCount += Time.deltaTime;
            if (_rushTimeCount >= _attackTime)
            {
                _rushing = false;
                _rushTimeCount = 0;
            }
        }
        else
        {
            _rb.velocity = Vector3.zero;
            CheckPlayerDistance();
        }
    }

    /// <summary>
    /// �v���C���[�Ƃ̋������`�F�b�N����
    /// </summary>
    private void CheckPlayerDistance()
    {
        if (!_checkPlayerDis) return;

        var distance = Vector3.Distance(this.transform.position, _player.transform.position);

        if (distance <= _attackDistance)
        {
            if (!_rushing)
            {
                _animator.SetTrigger(_attackStr);
                _checkPlayerDis = false;
                StartCoroutine(CheckPlayerCoolTime());
            }
        }
    }

    private IEnumerator CheckPlayerCoolTime()
    {
        yield return new WaitForSeconds(_checkCoolTime);
        _checkPlayerDis = true;
    }

    public void Rush()
    {
        _rushing = true;
    }
}
