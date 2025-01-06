using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField, Header("�Y�[�����̃X�s�[�h")]
    private float _zoomSpeed;
    [SerializeField, Header("�Ǐ]�̃X�s�[�h")]
    private float _followSpeed;
    [SerializeField,Header("Ray�̒���")]
    float _rayLength = 0.5f;

    [SerializeField]
    GameObject _player;//�v���C���[
    PlayerController_2D _playerController2D;

    Vector3 _offset;//�����̃v���C���[�ƃJ�����̍�
    Vector3 _zoomOffset;
    Vector3 _pos;

    float _firstDis;//�ŏ��̋���
    float _dis;//���݂̋���
    float _zoomValue;
    float _zoomObjTime;
    Vector3 _zoomObjPos;

    //�Y�[�������ǂ���
    private bool _isZoom = false;
    private bool _isFollowX = true;
    private bool _isFollowY = true;
    bool _isCameraShake = false;
    bool _isObjZoom = false;

    private Vector3 _previousPosition; // �O�̃t���[���̈ʒu

    // Start is called before the first frame update
    void Start()
    {
        _playerController2D = _player.GetComponent<PlayerController_2D>();
        _offset = _player.transform.position - this.transform.position;
        _pos = transform.position;
        _firstDis = Vector3.Distance(_player.transform.position, transform.position);
    }

    private void Update()
    {
        //�f�o�b�O
        if (Input.GetKeyDown(KeyCode.N))
        {
            Zoom(2);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            ZoomOut();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Shake(5, 0.1f);
        }

        ObjDetection();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_player != null)
        {
            if (_isObjZoom)
            {
                ZoomObjUpdate();
            }
            else
            {
                FollowUpdate();
                ZoomUpdate();
            }
        }
    }

    void FollowUpdate()
    {
        if (_isFollowY) _pos.y = Mathf.Lerp(_pos.y, _player.transform.position.y - _offset.y, _followSpeed);

        if (_isFollowX) _pos.x = Mathf.Lerp(_pos.x, _player.transform.position.x - _offset.x, _followSpeed);
        //_pos�̈ʒu�Ɉړ�
        transform.position = _pos;
    }

    void ZoomUpdate()
    {
        if (_isZoom)
        {
            _pos.z = Mathf.Lerp(_pos.z, _player.transform.position.z - _offset.z + _zoomValue, _zoomSpeed);
        }
        else
        {
            _pos.z = Mathf.Lerp(_pos.z, _player.transform.position.z - _offset.z, _zoomSpeed);
        }
    }

    void ZoomObjUpdate()
    {
        _pos = Vector3.Lerp(_pos, _zoomObjPos, _zoomSpeed);
        _zoomObjTime -= Time.deltaTime;
        if(_zoomObjTime <= 0)
        {
            ZoomOut();
        }
    }

    public void Zoom(float value)
    {
        if (_isZoom) return;
        _zoomValue = value;
        //�Y�[�����ɕύX
        _isZoom = true;
    }

    public void ZoomObj(float value, Vector3 pos, float time)
    {
        if (_isZoom) return;
        _zoomObjPos = pos;
        _zoomObjPos.z = _player.transform.position.z - _offset.z + _zoomValue;
        _zoomObjTime = time;
        //�Y�[�����ɕύX
        _isZoom = true;
        _isObjZoom = true;
    }

    public void ZoomOut()
    {
        if (!_isZoom) return;

        //�Y�[�����ł͂Ȃ���ԂɕύX
        _isZoom = false;
        _isObjZoom = false;
    }

    //�J�����h��i�f�t�H���g�l : 0.25f, 0.1f�j
    IEnumerator CameraShake(float duration, float magnitude)
    {
        //��ʗh�ꒆ������
        if (_isCameraShake) yield break;

        var _beforePos = transform.localPosition;

        var _beforeRot = transform.localRotation;

        var _elapsed = 0f;

        _isCameraShake = true;

        while (_elapsed < duration)
        {
            var x = _beforePos.x + Random.Range(-1f, 1f) * magnitude;
            var y = _beforePos.y + Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, _beforePos.z);
            _elapsed += Time.deltaTime;

            yield return null;
        }

        //��ʗh��O�̈ʒu�ɖ߂�
        transform.localPosition = _beforePos;
        transform.localRotation = _beforeRot;
        _isCameraShake = false;
    }

    //�~�܂��Ă��鎞�ɂ̂ݎg�p�i�J�b�g�V�[���I�ȁj
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(CameraShake(duration, magnitude));
    }

    //�J�����̐i�s�����ɃI�u�W�F�N�g����������i�܂Ȃ�
    void ObjDetection()
    {
        Vector3 directionX = new Vector3(_player.transform.position.x - _offset.x - transform.position.x,0, 0).normalized;
        Vector3 directionY = new Vector3(0, _player.transform.position.y - _offset.y - transform.position.y, 0).normalized;
        
        // �ړ�������Ray���΂��ď�Q�������m
        //X
        Ray rayX = new Ray(transform.position, directionX);
        // ��Q�����Ȃ���Έړ�
        if (!Physics.Raycast(rayX, _rayLength))
        {
            _isFollowX = true;
        }
        else
        {
            _isFollowX = false;
        }
        //Y
        Ray rayY = new Ray(transform.position, directionY);
        // ��Q�����Ȃ���Έړ�
        if (!Physics.Raycast(rayY, _rayLength))
        {
            _isFollowY = true;
        }
        else
        {
            _isFollowY = false;
        }
    }
}
