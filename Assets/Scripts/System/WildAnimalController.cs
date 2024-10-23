using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildAnimalController : MonoBehaviour
{
    //プレイヤー
    private PlayerController_2D _player;

    [SerializeField, Header("自分がなんの動物か")]
    private Animal _myType;
    [SerializeField, Header("動物")]
    GameObject _animalObj;
    BoxCollider _col;
    void Start()
    {
        //プレイヤーを取得
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
        //もしプレイヤーに当たったら
        if (other.gameObject.CompareTag("Player"))
        {
            //プレイヤーの変身を呼ぶ
            _player.ChangeAnimal(_myType);
            _animalObj.SetActive(false);
            _col.enabled = false;
        }
    }
}
