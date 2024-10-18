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
        new AnimalData(0f, 0f),//通常
        new AnimalData(0f, 0f),//ゴリラ
        new AnimalData(0f, 0f),//トカゲ
        new AnimalData(0f, 0f),//魚
        new AnimalData(0f, 0f),//リス
        new AnimalData(0f, 0f),//鹿
        new AnimalData(0f, 0f),//鳥
        new AnimalData(0f, 0f),//イカ
        new AnimalData(0f, 0f)//蛇
    };
}

