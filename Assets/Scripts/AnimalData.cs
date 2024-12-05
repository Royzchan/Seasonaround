using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalData
{
    public float Speed;
    public float JumpPower;
    public AnimalData(float speed, float jumpPower)
    {
        Speed = speed;
        JumpPower = jumpPower;
    }

    public static AnimalData[] animalDatas ={
        new AnimalData(5f, 5f),//�ʏ�
        new AnimalData(4f, 5f),//�S����
        new AnimalData(5f, 5f),//�g�J�Q
        new AnimalData(5f, 5f),//��
        new AnimalData(5f, 5f),//���X
        new AnimalData(8f, 10f),//��
        new AnimalData(5f, 5f),//��
        new AnimalData(5f, 5f),//�C�J
        new AnimalData(5f, 5f)//��
    };
}

