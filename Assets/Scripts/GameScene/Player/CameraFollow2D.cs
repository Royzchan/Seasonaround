using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField, Header("�Y�[�����̃X�s�[�h")]
    private float _zoomSpeed;
    [SerializeField, Header("�Ǐ]�̃X�s�[�h")]
    private float _followSpeed;

    [SerializeField]
    GameObject _player;//�v���C���[
    PlayerController_2D _playerController2D;

    Vector3 _offset;//�����̃v���C���[�ƃJ�����̍�
    Vector3 _zoomOffset;
    Vector3 _pos;

    float _firstDis;//�ŏ��̋���
    float _dis;//���݂̋���

    //�Y�[�������ǂ���
    private bool _isZoom = false;

    bool _isCameraShake = false;

    enum State
    {
        Follow,
        Zoom
    }
    State _state = State.Follow;

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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_player != null)
        {
            if (_state == State.Follow)
            {
                FollowUpdate();
            }
            else if (_state == State.Zoom)
            {
                ZoomUpdate();
            }
        }
    }

    void FollowUpdate()
    {
        if (_isZoom)
        {
            // �J�����̒Ǐ]
           
            _pos.y = _player.transform.position.y - _offset.y;
            
            _pos.x = _player.transform.position.x - _zoomOffset.x;
        }
        else
        {
            // �J�����̒Ǐ]
            
            _pos.y = _player.transform.position.y - _offset.y;

            _pos.x = _player.transform.position.x - _offset.x;
        }
        //_pos�̈ʒu�Ɉړ�
        transform.position = Vector3.Lerp(transform.position, _pos, _followSpeed);
    }

    void ZoomUpdate()
    {
        if (_isZoom)
        {
            _pos.x = _player.transform.position.x;
            if (_playerController2D.ISFly) _pos.y = _player.transform.position.y - _offset.y;
        }
        else
        {
            _pos.x = _player.transform.position.x - _offset.x;
            if (_playerController2D.ISFly) _pos.y = _player.transform.position.y - _offset.y;
        }

        //_pos�̈ʒu�Ɉړ�
        transform.position = Vector3.Lerp(transform.position, _pos, _zoomSpeed);
        if (transform.position == _pos)
        {
            _state = State.Follow;
            _zoomOffset = transform.position - transform.position;
        }
    }

    public void Zoom(float value)
    {
        if (_isZoom) return;

        _state = State.Zoom;
        _pos.z = _player.transform.position.z - _offset.z + value;
        //�Y�[�����ɕύX
        _isZoom = true;
    }

    public void ZoomOut()
    {
        if (!_isZoom) return;

        _pos.z = _player.transform.position.z - _offset.z;

        //�Y�[�����ł͂Ȃ���ԂɕύX
        _isZoom = false;
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
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(CameraShake(duration, magnitude));
    }
}
