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
        new AnimalData(0f, 0f),//�ʏ�
        new AnimalData(0f, 0f),//�S����
        new AnimalData(0f, 0f),//�g�J�Q
        new AnimalData(0f, 0f),//��
        new AnimalData(0f, 0f),//���X
        new AnimalData(0f, 0f),//��
        new AnimalData(0f, 0f),//��
        new AnimalData(0f, 0f),//�C�J
        new AnimalData(0f, 0f)//��
    };
}

