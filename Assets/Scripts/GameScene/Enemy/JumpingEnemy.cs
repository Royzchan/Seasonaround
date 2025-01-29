using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemy : MonoBehaviour
{
    private Rigidbody _rb;
    GameObject _player;
    private PlayerController_2D _playerController;

    //�A�j���[�^�[
    [SerializeField]
    private Animator _animator;

    [SerializeField, Header("�v���C���[�Ƃ̋���")]
    private float _distance = 5f;
    [SerializeField, Header("�W�����v��")]
    private float _jumpPower = 5.0f;
    [SerializeField, Header("�W�����v�ł����")]
    private int _canJumpNum = 1;
    //�W�����v�̉�
    private int _jumpNum = 0;
    [SerializeField, Header("�W�����v�̃N�[���^�C��")]
    private float _jumpCoolTime = 1.0f;
    private bool _canJump = true;

    //�U���̃A�j���[�V�����̔���
    private string _isJump = "JumpNow";
    private string _dieStr = "Die";

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        if (_player != null) _playerController = _player.GetComponent<PlayerController_2D>();
        _rb = GetComponent<Rigidbody>();
        _animator.SetBool(_isJump, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_player != null)
        {
            float _dis = Vector3.Distance(this.transform.position, _player.transform.position);
            Jump(_dis);
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

    void Jump(float dis)
    {
        if (dis <= _distance)
        {
            //�W�����v����
            if (_jumpNum < _canJumpNum && _canJump)
            {
                _jumpNum++;
                _canJump = false;
                //�W�����v�̃A�j���[�V����
                JumpAnimation();
            }
        }
    }

    public void AddJumpPower()
    {
        _rb.AddForce(0f, _jumpPower, 0f, ForceMode.Impulse);
    }

    private IEnumerator StartCoolTime()
    {
        yield return new WaitForSeconds(_jumpCoolTime);
        _jumpNum = 0;
        _canJump = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if(!_canJump)
            {
                StartCoroutine(StartCoolTime());
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            //�v���C���[����������ɂ��邩
            bool playerIsUp = _player.transform.position.y >= this.transform.position.y;
            if (playerIsUp)
            {
                //���g�̎��S����
                Die();
            }
            else
            {
                //�v���C���[�̃_���[�W����
            }
        }
    }

    void JumpAnimation()
    {
        _animator.SetTrigger(_isJump);
    }

    void Die()
    {
        _animator.SetTrigger(_dieStr);
    }

    public void DeleteObj()
    {
        Destroy(this.gameObject);
    }
}
