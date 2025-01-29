using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemy : MonoBehaviour
{
    private Rigidbody _rb;

    GameObject player;

    //�A�j���[�^�[
    [SerializeField]
    private Animator _animator;

    //�U���̃A�j���[�V�����̔���
    private string _isJump = "JumpNow";

    [SerializeField, Header("�v���C���[�Ƃ̋���")]
    private float _distance = 5f;

    [SerializeField, Header("�W�����v��")]
    private float _jumpPower = 5.0f;

    //�W�����v�̉�
    private int _jumpNum = 0;

    [SerializeField, Header("�W�����v�ł����")]
    private int _canJumpNum = 1;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        _rb = GetComponent<Rigidbody>();
        _animator.SetBool(_isJump, false);
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            float _dis = Vector3.Distance(this.transform.position, player.transform.position);
            Jump(_dis);
        }

        if (transform.position.x > player.transform.position.x)
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

    void Jump(float dis)
    {
        if (dis <= _distance)
        {
            //�W�����v����
            if (_jumpNum < _canJumpNum)
            {
                _rb.AddForce(0f, _jumpPower, 0f, ForceMode.Impulse);
                _jumpNum++;
                //�W�����v�̃A�j���[�V����
                Invoke("JumpAnimation", 0.5f);
            }
        }
        else // �v���C���[�Ƃ̋������ݒ肵���͈͂𒴂����ꍇ
        {
            _animator.SetBool(_isJump, false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _jumpNum = 0;
        }
    }

    void JumpAnimation()
    {
        _animator.SetBool(_isJump, true);
    }
}
