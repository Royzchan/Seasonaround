using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSwitch : MonoBehaviour
{
    [SerializeField, Header("�A�N�e�B�u�ɂ������I�u�W�F�N�g")]
    public GameObject _targetObject;
    [SerializeField, Header("�X�C�b�`�̏����ʒu")]
    private Vector3 _initialPosition;
    [SerializeField, Header("�������ދ���")]
    public float _pushDistance = 0.1f;
    [SerializeField, Header("�������ޑ���")]
    public float _pushSpeed = 0.2f;
    [SerializeField, Header("�X�C�b�`�������ꂽ���ǂ���")]
    private bool isPushed = false;

    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = transform.position;  // �X�C�b�`�̏����ʒu��ۑ�
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isPushed && collision.gameObject.CompareTag("Player"))  // �v���C���[���X�C�b�`�ɐG�ꂽ��
        {
            isPushed = true;
            StartCoroutine(PushSwitch());  // �X�C�b�`�������A�j���[�V�������J�n
        }
    }

    IEnumerator PushSwitch()
    {
        Vector3 targetPosition = _initialPosition - new Vector3(0, _pushDistance, 0);  // �������ވʒu
        float elapsedTime = 0f;

        // �������ރA�j���[�V����
        while (elapsedTime < _pushSpeed)
        {
            transform.position = Vector3.Lerp(_initialPosition, targetPosition, elapsedTime / _pushSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;  // �ŏI�I�ɉ������񂾈ʒu�ɐݒ�

        // �I�u�W�F�N�g���A�N�e�B�u�ɂ���
        _targetObject.SetActive(true);
    }
}
