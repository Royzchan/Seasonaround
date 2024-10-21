using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPImage : MonoBehaviour
{
    PlayerController_2D _playerController2D;

    List<GameObject> _hpImages = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        _playerController2D = GameObject.FindWithTag("Player").GetComponent<PlayerController_2D>();

        // 子オブジェクトを全て取得する
        foreach (Transform child in this.transform)
        {
            _hpImages.Add(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < _hpImages.Count; i++)
        {
            if(i < _playerController2D.HP)
            {
                _hpImages[i].SetActive(true);
            }
            else
            {
                _hpImages[i].SetActive(false);
            }
        }
    }
}
