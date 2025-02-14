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

    [SerializeField, Header("�ːi����")]
    private float _rushDuration = 3f;  // �ːi���鎞�ԁi�b�j

    private float _waitCount = 0f; // �ҋ@���Ԃ̃^�C�}�[
    private float _pauseTimer = 0f; // ��~���̃^�C�}�[
    private float _rushTimer = 0f; // �ːi���̌o�ߎ���
    private bool isRushing = false;
    private bool isStandbay = false;
    private float rushDirectionX; // X�������݂̂̓ːi����
    private Vector3 rushStartPosition; // �ːi�J�n���̈ʒu

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

        if (!isRushing)
        {
            if (transform.position.x > player.transform.position.x)
            {
                // X�����v���C���[��菬�����ꍇ�A�X�P�[���𔽓]
                transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            }
            else
            {
                // ���̃X�P�[���ɖ߂�
                transform.localScale = new Vector3(1.3f, 1.3f, -1.3f);
            }
        }
    }

    void GoAttack()
    {
        isRushing = true;

        // �ːi����������i�v���C���[�̈ʒu����j
        rushDirectionX = player.transform.position.x > transform.position.x ? 1f : -1f;

        print("�ːi�J�n");
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
                Invoke("GoAttack", 1f);
            }
        }
        else // �v���C���[�Ƃ̋������ݒ肵���͈͂𒴂����ꍇ
        {
            isRushing = false;
            _animator.SetBool(_isAttack, false);
            _pauseTimer = _pauseDuration; // �ꎞ��~�^�C�}�[��ݒ�
        }

        // �ːi���͍ŏ��Ɍ��߂������Ɍ������Ĉړ���������
        if (isRushing)
        {
            // �ːi���Ԃ��o�߂�����ːi���~
            if (_rushTimer >= _rushDuration)
            {
                isRushing = false;
                _pauseTimer = _pauseDuration; // �ꎞ��~�^�C�}�[��ݒ�
                print("�ːi�I���i���Ԍo�߁j");

                // �v���C���[�̈ʒu�Ɋ�Â��ēːi����������i�ŏ��̈�x����)
                rushDirectionX = player.transform.position.x > transform.position.x ? 1f : -1f;
                rushStartPosition = transform.position; // �ːi�J�n���̈ʒu���L�^
                _rushTimer = 0f; // �ːi���Ԃ̃��Z�b�g
            }
            else
            {
                // �ːi���Ɍo�ߎ��Ԃ𑝉�
                _rushTimer += Time.deltaTime;
                transform.position += new Vector3(rushDirectionX * _rushSpeed * Time.deltaTime, 0f, 0f);
            }
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
        }
    }
}
