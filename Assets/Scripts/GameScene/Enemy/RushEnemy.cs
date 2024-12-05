using UnityEngine;

public class RushEnemy : MonoBehaviour
{
    GameObject player;
    Rigidbody rb;

    //�A�j���[�^�[
    [SerializeField]
    private Animator _animator;

    //�U���̃A�j���[�V�����̔���
    private string _isAttack = "AttackNow";

    [SerializeField, Header("�v���C���[�Ƃ̋���")]
    private float _distance = 5f;

    [SerializeField, Header("�ːi���x")]
    private float _rushSpeed = 10f;

    [SerializeField, Header("�v���C���[�������߂���")]
    private float _knockbackPower = 10f;

    [SerializeField, Header("�ːi���~���鎞��")]
    private float _pauseDuration = 2f;

    [SerializeField, Header("�ҋ@����")]
    private float _waitTime = 1f;

    private float _waitCount = 0f; // �ҋ@���Ԃ̃^�C�}�[
    private float _pauseTimer = 0f; // ��~���̃^�C�}�[
    private bool isRushing = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        _pauseTimer = _pauseDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            float _dis = Vector3.Distance(this.transform.position, player.transform.position);
            Rush(_dis);
        }
    }

    void GoAttack()
    {
        isRushing = true; // �ːi���J�n
    }

    // �ːi�֐�
    void Rush(float dis)
    {
        if (dis <= _distance)
        {
            // �ꎞ��~�^�C�}�[�̍X�V
            if (_pauseTimer > 0f)
            {
                _pauseTimer -= Time.deltaTime; // �^�C�}�[�����炷
            }

            if (!isRushing && _pauseTimer <= 0f) // �ꎞ��~���łȂ��ꍇ�ɓːi���J�n
            {
                //�ːi�̃A�j���[�V�����J�n
                _animator.SetBool(_isAttack, true);
                print("�ːi�J�n");
            }
        }
        else // �v���C���[�Ƃ̋������ݒ肵���͈͂𒴂����ꍇ
        {
            isRushing = false;
            _animator.SetBool(_isAttack, false);
            _pauseTimer = _pauseDuration; // �ꎞ��~�^�C�}�[��ݒ�
        }

        // �v���C���[�̕����Ɍ������Ĉړ�
        if (isRushing)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.position += direction * _rushSpeed * Time.deltaTime;
        }
    }

    // �Փˎ��̏���
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // �ːi���~����
            isRushing = false;
            _pauseTimer = _pauseDuration; // �ꎞ��~�^�C�}�[��ݒ�

            // �v���C���[�����ɏ���������
            Vector3 direction = (player.transform.position - transform.position).normalized;
            player.transform.position += direction * _knockbackPower * Time.deltaTime;

            print("�ːi��~�i�v���C���[�ƏՓˁj");
        }
    }
}
