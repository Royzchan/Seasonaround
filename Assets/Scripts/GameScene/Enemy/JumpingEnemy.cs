using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class JumpingEnemy : MonoBehaviour
{
    private Rigidbody _rb;
    GameObject _player;
    PlayerController_2D _playerController;
    private Animator _animator;

    [SerializeField, Header("�v���C���[�Ƃ̋���")]
    private float _distance = 5f;
    [SerializeField, Header("�W�����v��")]
    private float _jumpPower = 5.0f;
    [SerializeField, Header("�W�����v�ł����")]
    private int _canJumpNum = 1;
    [SerializeField, Header("�W�����v�̃N�[���^�C��")]
    private float _jumpCoolTime = 1.0f;
    private int _jumpNum = 0; // �W�����v������
    private float _jumpCoolTimeCounter = 0f; // �W�����v�̃N�[���^�C���̃J�E���^�[
    private bool _canJump = true; // �W�����v�\�t���O
    private bool _onGround = true;

    //�A�j���[�V����
    private string _isJump = "Jump"; // �W�����v�̃A�j���[�V�����̌Ăяo���L�[
    private string _isDie = "Die";   // ���S�̃A�j���[�V�����̌Ăяo���L�[

    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _playerController = FindAnyObjectByType<PlayerController_2D>();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player != null)
        {
            float dis = Vector3.Distance(this.transform.position, _player.transform.position);
            if (dis <= _distance)
            {
                if (_canJump) Jump();
            }

            if (transform.position.x > _player.transform.position.x)
            {
                // X�����v���C���[��菬�����ꍇ�A�X�P�[���𔽓]
                transform.localScale = new Vector3(50f, 50f, 50f);
            }
            else
            {
                // ���̃X�P�[���ɖ߂�
                transform.localScale = new Vector3(50f, 50f, -50f);
            }
        }
    }

    /// <summary>
    /// �W�����v����
    /// </summary>
    void Jump()
    {
        if (_jumpNum < _canJumpNum && _onGround)
        {
            _canJump = false;
            StartCoroutine(StartJumpCoolTime());
            _animator.SetTrigger(_isJump);
        }
    }

    /// <summary>
    /// �W�����v�̗͂������鏈��
    /// </summary>
    public void AddJumpForce()
    {
        _rb.AddForce(0f, _jumpPower, 0f, ForceMode.Impulse);
    }

    private IEnumerator StartJumpCoolTime()
    {
        yield return new WaitForSeconds(_jumpCoolTime);
        _canJump = true;
    }

    private void Die()
    {
        _animator.SetTrigger(_isDie);
    }

    public void DeleteObj()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //�ڒn�����^�C�~���O�Œn�ʂɂ��锻���true��
            _onGround = true;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            if (_player.transform.position.y >= this.transform.position.y)
            {
                Die();
            }
            else
            {
                if (_playerController != null) _playerController.Damage(1);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //�n�ʂ��痣�ꂽ��n�ʂɂ��锻���false��
            _onGround = false;
        }
    }
}
