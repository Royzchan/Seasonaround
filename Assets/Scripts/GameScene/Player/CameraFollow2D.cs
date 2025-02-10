using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings; // UnityEditor �� PlayerSettings ���g�p���Ă��邪�A�Q�[�����s���ɂ͕s�v�Ȃ��߁A�폜������

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField, Header("�J�����̃Y�[�����x")]
    private float _zoomSpeed;
    [SerializeField, Header("�Ǐ]���鑬�x")]
    private float _followSpeed;
    [SerializeField, Header("���C�L���X�g�̒���")]
    float _rayLength = 0.5f;

    [SerializeField]
    GameObject _player;
    PlayerController_2D _playerController2D;

    Vector3 _offset;//�����̃v���C���[�ƃJ�����̍�
    Vector3 _zoomPos; // �Y�[�����̖ڕW�ʒu
    Vector3 _pos; // �J�����̌��݂̈ʒu�iLerp�p�j

    float _firstDis; // �J�����ƃv���C���[�̏�������
    float _dis; // �J�����ƃv���C���[�̌��݂̋���
    float _zoomValue; // �Y�[�����ɕύX����Z���̒l
    float _zoomObjSpeed = 0.05f; // �I�u�W�F�N�g�Y�[�����̑��x
    float _zoomObjDistance;

    private bool _isZoom = false; // ���݃Y�[�������ǂ����̃t���O
    private bool _isFollowX = true; // X�������ւ̒Ǐ]���s�����ǂ����̃t���O
    private bool _isFollowY = true; // Y�������ւ̒Ǐ]���s�����ǂ����̃t���O
    bool _isCameraShake = false; // �J�����V�F�C�N�����ǂ����̃t���O
    bool _isObjZoom = false; // �I�u�W�F�N�g�Y�[�������ǂ����̃t���O

    // Start is called before the first frame update
    void Start()
    {
        _playerController2D = _player.GetComponent<PlayerController_2D>();
        _offset = _player.transform.position - this.transform.position;
        _pos = transform.position; // �J�����̏����ʒu��ۑ�
        _firstDis = Vector3.Distance(_player.transform.position, transform.position);
    }

    private void Update()
    {
        ObjDetection(); // ��Q�����o�𖈃t���[�����s
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_player != null)
        {
            if (_isObjZoom)
            {
                ZoomObjUpdate(); // �I�u�W�F�N�g�Y�[�����̃J�����ړ�����
            }
            else
            {
                FollowUpdate(); // �ʏ�̃J�����Ǐ]����
                ZoomUpdate(); // �ʏ�̃J�����Y�[������
            }
        }
    }

    // �J�������v���C���[�ɒǏ]�����鏈��
    void FollowUpdate()
    {
        // Y�������ւ̒Ǐ]���L���ȏꍇ�A�J������Y���W���v���C���[�ɍ��킹��Lerp�ňړ�
        if (_isFollowY) _pos.y = Mathf.Lerp(_pos.y, _player.transform.position.y - _offset.y, _followSpeed);

        // X�������ւ̒Ǐ]���L���ȏꍇ�A�J������X���W���v���C���[�ɍ��킹��Lerp�ňړ�
        if (_isFollowX) _pos.x = Mathf.Lerp(_pos.x, _player.transform.position.x - _offset.x, _followSpeed);
        //_pos�̈ʒu�Ɉړ�
        transform.position = _pos; // �J�������v�Z�����ʒu�Ɉړ�
    }

    // �J�����̃Y�[������
    void ZoomUpdate()
    {
        // �Y�[�����̏ꍇ�A�J������Z���W��ڕW�l�܂�Lerp�ňړ�
        if (_isZoom)
        {
            _pos.z = Mathf.Lerp(_pos.z, _player.transform.position.z - _offset.z + _zoomValue, _zoomSpeed);
        }
        // �Y�[�����łȂ��ꍇ�A�J������Z���W�������l�܂�Lerp�ňړ�
        else
        {
            _pos.z = Mathf.Lerp(_pos.z, _player.transform.position.z - _offset.z, _zoomSpeed);
        }
    }

    // �I�u�W�F�N�g�Y�[�����̃J�����ړ�����
    void ZoomObjUpdate()
    {
        // �Y�[�����̏ꍇ
        if (_isZoom)
        {
            //_zoomObjPos�ɋ߂Â��i_pos.z�݂̂�_zoomValue�𑫂������̂Ōʁj
            _pos = Vector3.Lerp(_pos, _zoomPos - _offset, _zoomObjSpeed);
            //_pos�̈ʒu�Ɉړ�
            transform.position = _pos; // �v�Z�����ʒu�ɃJ�������ړ�
        }
        if (!_isZoom)
        {
            _pos = Vector3.Lerp(_pos, _player.transform.position - _offset, _zoomObjSpeed); // �J�������v���C���[�̈ʒu�ɖ߂�
            //_pos�̈ʒu�Ɉړ�
            transform.position = _pos; // �v�Z�����ʒu�ɃJ�������ړ�

            // �ڕW�n�_�ɓ��B�������`�F�b�N
            if (Vector3.Distance(transform.position, _player.transform.position - _offset) < 0.01f) // �������l��ݒ�
            {
                ObjZommFinish(); // �Y�[���I�������𑦎��Ăяo��
            }
        }
    }

    // �I�u�W�F�N�g�Y�[���I������
    void ObjZommFinish()
    {
        //�v���C���[��������悤��
        _playerController2D.InputActionEnable(); // �v���C���[�̓��͂�L���ɂ���
        _isObjZoom = false; // �I�u�W�F�N�g�Y�[���t���O������
    }

    // �J�������Y�[���C��������
    public void Zoom(float value)
    {
        if (_isZoom) return; // �Y�[�����̏ꍇ�͏����𒆒f
        _zoomValue = value; // �Y�[���l��ݒ�
        //�Y�[�����ɕύX
        _isZoom = true; // �Y�[���t���O�𗧂Ă�
    }

    // ����̃I�u�W�F�N�g�ɑ΂��ăY�[���C������
    public void ZoomObj(float value, Vector3 pos, float _speed)
    {
        _zoomValue = value; // �Y�[���l��ݒ�
        _zoomPos = pos; // �Y�[�����̖ڕW�ʒu��ݒ�
        _zoomPos.z += _zoomValue;//z���̃Y�[���ʂ����Z
        _zoomObjSpeed = _speed; // �I�u�W�F�N�g�Y�[�����̑��x��ݒ�
        //�Y�[�����ɕύX
        _isZoom = true; // �Y�[���t���O�𗧂Ă�
        _isObjZoom = true; // �I�u�W�F�N�g�Y�[���t���O�𗧂Ă�
    }

    // �J�������Y�[���A�E�g������
    public void ZoomOut()
    {
        if (!_isZoom) return; // �Y�[�����łȂ��ꍇ�͏����𒆒f

        //�Y�[�����ł͂Ȃ���ԂɕύX
        _isZoom = false; // �Y�[���t���O������
    }

    // �J�����h��i�f�t�H���g�l : 0.25f, 0.1f�j
    IEnumerator CameraShake(float duration, float magnitude)
    {
        //��ʗh�ꒆ������
        if (_isCameraShake) yield break; // �J�����V�F�C�N���̏ꍇ�͏����𒆒f

        var _beforePos = transform.localPosition; // �V�F�C�N�O�̃J�����ʒu��ۑ�

        var _beforeRot = transform.localRotation; // �V�F�C�N�O�̃J������]��ۑ�

        var _elapsed = 0f; // �o�ߎ��Ԃ�������

        _isCameraShake = true; // �J�����V�F�C�N�t���O�𗧂Ă�

        // �V�F�C�N���Ԃ̊�
        while (_elapsed < duration)
        {
            // �����_���Ȓl��ݒ肵�ăJ������h�炷
            var x = _beforePos.x + Random.Range(-1f, 1f) * magnitude;
            var y = _beforePos.y + Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, _beforePos.z); // �J�����̈ʒu���X�V
            _elapsed += Time.deltaTime; // �o�ߎ��Ԃ����Z

            yield return null;
        }

        //��ʗh��O�̈ʒu�ɖ߂�
        transform.localPosition = _beforePos; // �J���������̈ʒu�ɖ߂�
        transform.localRotation = _beforeRot; // �J���������̉�]�ɖ߂�
        _isCameraShake = false; // �J�����V�F�C�N�t���O������
    }

    //�~�܂��Ă��鎞�ɂ̂ݎg�p�i�J�b�g�V�[���I�ȁj
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(CameraShake(duration, magnitude)); // �J�����V�F�C�N�R���[�`�����J�n
    }

    //�J�����̐i�s�����ɃI�u�W�F�N�g����������i�܂Ȃ�
    void ObjDetection()
    {
        Vector3 directionX = new Vector3(_player.transform.position.x - _offset.x - transform.position.x, 0, 0).normalized; // X�������ւ̃��C�L���X�g�̕���
        Vector3 directionY = new Vector3(0, _player.transform.position.y - _offset.y - transform.position.y, 0).normalized; // Y�������ւ̃��C�L���X�g�̕���

        Ray rayX = new Ray(transform.position, directionX); // X�������ւ̃��C�𐶐�
        RaycastHit hitX;
        if (Physics.Raycast(rayX, out hitX, _rayLength)) // ���C����Q���ɓ��������ꍇ
        {
            if (hitX.collider.gameObject.CompareTag("Water"))
            {
                return;
            }
            _isFollowX = false; // X�������ւ̒Ǐ]���֎~
        }
        else
        {
            _isFollowX = true; // X�������ւ̒Ǐ]������
        }

        Ray rayY = new Ray(transform.position, directionY); // Y�������ւ̃��C�𐶐�
        Debug.DrawRay(transform.position, directionY, Color.red, 1.0f);
        RaycastHit hitY;
        if (Physics.Raycast(rayY, out hitY, _rayLength)) // ���C����Q���ɓ��������ꍇ
        {
            if (hitY.collider.gameObject.CompareTag("Water"))
            {
                return;
            }
            _isFollowY = false; // Y�������ւ̒Ǐ]���֎~
            Debug.Log(hitY.collider.gameObject.name);
        }
        else
        {
            _isFollowY = true; // Y�������ւ̒Ǐ]������
        }
    }
}