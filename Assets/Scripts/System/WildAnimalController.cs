using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildAnimalController : MonoBehaviour
{
    //�v���C���[
    private PlayerController_2D _player;

    [SerializeField, Header("�������Ȃ�̓�����")]
    private Animal _myType;
    [SerializeField, Header("����")]
    GameObject _animalObj;
    BoxCollider _col;
    void Start()
    {
        //�v���C���[���擾
        _player = FindAnyObjectByType<PlayerController_2D>();
        _col = GetComponent<BoxCollider>();
        _animalObj = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        _animalObj.transform.Rotate(0,20 * Time.deltaTime,0);
    }

    private void OnTriggerEnter(Collider other)
    {
        //�����v���C���[�ɓ���������
        if (other.gameObject.CompareTag("Player"))
        {
            //�v���C���[�̕ϐg���Ă�
            _player.ChangeAnimal(_myType);
            _animalObj.SetActive(false);
            _col.enabled = false;
        }
    }
}
