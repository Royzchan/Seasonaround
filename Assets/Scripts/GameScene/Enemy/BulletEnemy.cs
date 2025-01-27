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

    // �A�j���[�V�����̃X�e�[�^�X
    private string _attackStr = "isAttack";

    // �ҋ@���̏㉺�̓����Ɋւ���ϐ�
    [SerializeField, Header("�ҋ@���̏㉺�ړ��͈̔�")]
    private float _moveRange = 0.5f;
    [SerializeField, Header("�㉺�ړ��̑���")]
    private float _moveSpeed = 2f;
    private float _initialY;

    private bool isAttacking = false;  // �U�������ǂ����̃t���O

    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _animator = GetComponent<Animator>();
        _initialY = transform.position.y; // ����Y���W��ۑ�
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

        MoveUpAndDown();
    }

    // �U��
    public void Attack()
    {
        // �e�𐶐�
        Instantiate(bulletPrefab, bulletPosition.position, bulletPosition.rotation);
        isAttacking = true;  // �U�����t���O���I��
    }

    // �����`�F�b�N
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
        isAttacking = false;  // �U���I�����Ƀt���O���I�t
    }

    // �ҋ@���̏㉺�ړ�����
    private void MoveUpAndDown()
    {
        float newY = _initialY + Mathf.Sin(Time.time * _moveSpeed) * _moveRange;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    // �v���C���[�ɓ��܂ꂽ�Ƃ��̏���
    private void OnCollisionEnter(Collision collision)
    {
        // �v���C���[���G�ɐڐG�����ꍇ
        if (collision.gameObject.CompareTag("Player"))
        {
            // �v���C���[���G�̏�ɏ�������ǂ������m�F
            if (collision.contacts[0].point.y > transform.position.y)
            {
                Die();  // �ォ�瓥�܂ꂽ�ꍇ�͓G������
            }
        }
    }

    // �G�����S���鏈��
    private void Die()
    {
        Destroy(gameObject);  // �G������
    }
}
