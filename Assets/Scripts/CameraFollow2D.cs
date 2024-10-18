using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField,Header("�Y�[�����̃X�s�[�h")]
    private float _zoomSpeed;
    [SerializeField,Header("�Ǐ]�̃X�s�[�h")]
    private float _followSpeed;

    GameObject _player;//�v���C���[

    Vector3 _offset;//�����̃v���C���[�ƃJ�����̍�
    Vector3 _pos;

    float _firstDis;//�ŏ��̋���
    float _dis;//���݂̋���

    //�Y�[�������ǂ���
    private bool isZoom = false;

    // ���ݑ��x(SmoothDamp�̌v�Z�̂��߂ɕK�v)
    private float _currentVelocity = 0;

    // �ڕW�l�ɓ��B����܂ł̂����悻�̎���[s]
    [SerializeField] private float _smoothTime = 0.3f;

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
        //�Y�[���i���j
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartCoroutine(Zoom(3));
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(ZoomOut());
        }
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
        if (isZoom) yield break; 

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
        isZoom = true;
    }

    public IEnumerator ZoomOut()
    {
        if (!isZoom) yield break;

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
        isZoom = false;
    }
}
