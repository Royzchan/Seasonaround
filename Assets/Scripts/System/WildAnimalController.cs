using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildAnimalController : MonoBehaviour
{
    //プレイヤー
    private PlayerController_2D _player;

    [SerializeField, Header("自分がなんの動物か")]
    private Animal _myType;

    void Start()
    {
        //プレイヤーを取得
        _player = FindAnyObjectByType<PlayerController_2D>();
    }

    void Update()
    {

    }

    private void nTriggerEnter(Collider other)
    {
        //もしプレイヤーに当たったら
        if (other.gameObject.CompareTag("Player"))
        {
            //プレイヤーの変身を呼ぶ
            _player.ChangeAnimal(_myType);
        }
    }
}
