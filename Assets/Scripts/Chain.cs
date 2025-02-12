using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SelectManager;

public class Chain : MonoBehaviour
{
    [SerializeField,Header("t‰ÄH“~‚Ì‡‚Å“ü‚ê‚é")] GameObject[] _chains;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < _chains.Length; i++)
        {
            _chains[i].SetActive(true);
        }

        if (PlayerPrefs.GetInt(_seasonKeys[SeasonEnum.Summer]) != 0)
        {
            _chains[0].SetActive(false);
        }
        if(PlayerPrefs.GetInt(_seasonKeys[SeasonEnum.Autumn]) != 0){
            _chains[1].SetActive(false);
        }
        if (PlayerPrefs.GetInt(_seasonKeys[SeasonEnum.Winter]) != 0)
        {
            _chains[2].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
