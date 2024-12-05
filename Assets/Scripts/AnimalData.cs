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
        new AnimalData(5f, 5f),//通常
        new AnimalData(4f, 5f),//ゴリラ
        new AnimalData(5f, 5f),//トカゲ
        new AnimalData(5f, 5f),//魚
        new AnimalData(5f, 5f),//リス
        new AnimalData(8f, 10f),//鹿
        new AnimalData(5f, 5f),//鳥
        new AnimalData(5f, 5f),//イカ
        new AnimalData(5f, 5f)//蛇
    };
}

