using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField, Header("�Y�[�����̃X�s�[�h")]
    private float _zoomSpeed;
    [SerializeField, Header("�Ǐ]�̃X�s�[�h")]
    private float _followSpeed;

    GameObject _player;//�v���C���[

    Vector3 _offset;//�����̃v���C���[�ƃJ�����̍�
    Vector3 _pos;

    float _firstDis;//�ŏ��̋���
    float _dis;//���݂̋���

    //�Y�[�������ǂ���
    private bool _isZoom = false;

    bool _isCameraShake = false;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _offset = _player.transform.position - this.transform.position;
        _pos = transform.position;
        _firstDis = Vector3.Distance(_player.transform.position, transform.position);
    }

    private void Update()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_player != null)
        {
            // �J�����̒Ǐ]
            _pos.x = _player.transform.position.x - _offset.x;

            //_pos�̈ʒu�Ɉړ�
            transform.position = Vector3.Lerp(transform.position, _pos, _followSpeed);
        }
    }

    public IEnumerator Zoom(float value)
    {
        if (_isZoom) yield break;

        //���݂̋������v�Z
        _dis = Vector3.Distance(_player.transform.position, transform.position);

        while (_firstDis - value < _dis)
        {
            //���݂̋������v�Z
            _dis = Vector3.Distance(_player.transform.position, transform.position);
            //�Y�[��
            _pos.z += _zoomSpeed * Time.deltaTime;
            yield return null;
        }

        //�Y�[�����ɕύX
        _isZoom = true;
    }

    public IEnumerator ZoomOut()
    {
        if (!_isZoom) yield break;

        //���݂̋������v�Z
        _dis = Vector3.Distance(_player.transform.position, transform.position);

        while (_firstDis > _dis)
        {
            //���݂̋������v�Z
            _dis = Vector3.Distance(_player.transform.position, transform.position);
            //�Y�[���A�E�g
            _pos.z -= _zoomSpeed * Time.deltaTime;
            yield return null;
        }

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
