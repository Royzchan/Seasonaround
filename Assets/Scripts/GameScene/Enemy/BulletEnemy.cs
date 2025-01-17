using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    GameObject _player;
    private Animator _animator;

    [SerializeField, Header("�U�����n�߂鋗��")]
    private float _attackDistance = 5f;
    [SerializeField, Header("�U���̊Ԋu")]
    private float _attackCoolTime = 3.0f;
    private float _coolTimeCount = 0;

    [SerializeField, Header("�e�̃I�u�W�F�N�g")]
    private GameObject bulletPrefab;

    [SerializeField, Header("�e�̐����ꏊ")]
    private Transform bulletPosition;

    private bool _canAttack = true;

    //�A�񃁁[�V�����̃X�e�[�^�X
    private string _attackStr = "isAttack";

    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!_canAttack)
        {
            _coolTimeCount += Time.deltaTime;
            if (_coolTimeCount >= _attackCoolTime)
            {
                _coolTimeCount = 0;
                _canAttack = true;
            }
        }

        if (_player != null)
        {
            // �v���C���[�ƓG�̈ʒu�֌W�p
            float _compare = this.transform.position.x - _player.transform.position.x;
            LeftorRight(_compare);

            CheckPlayerDistance();
        }
    }

    //�U��
    public void Attack()
    {
        //�e�𐶐�
        Instantiate(bulletPrefab, bulletPosition.position, bulletPosition.rotation);
    }

    //�����`�F�b�N
    private void CheckPlayerDistance()
    {
        var myPos = this.transform.position;
        var playerPos = _player.transform.position;
        var distance = Vector3.Distance(myPos, playerPos);
        if (distance <= _attackDistance)
        {
            if (_canAttack)
            {
                _animator.SetBool(_attackStr, true);
                _canAttack = false;
                _coolTimeCount = 0;
            }
        }
    }

    // ���������E������
    void LeftorRight(float compare)
    {
        if (compare > 0)
        {
            this.transform.eulerAngles = new Vector3(0, -90, 0);
        }
        else
        {
            this.transform.eulerAngles = new Vector3(0, 90, 0);
        }
    }

    public void EndAttackAnim()
    {
        _animator.SetBool(_attackStr, false);
    }
}
