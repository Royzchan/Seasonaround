using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildAnimalController : MonoBehaviour
{
    //�v���C���[
    private PlayerController_2D _player;

    [SerializeField, Header("�������Ȃ�̓�����")]
    private Animal _myType;

    void Start()
    {
        //�v���C���[���擾
        _player = FindAnyObjectByType<PlayerController_2D>();
    }

    void Update()
    {

    }

    private void nTriggerEnter(Collider other)
    {
        //�����v���C���[�ɓ���������
        if (other.gameObject.CompareTag("Player"))
        {
            //�v���C���[�̕ϐg���Ă�
            _player.ChangeAnimal(_myType);
        }
    }
}
