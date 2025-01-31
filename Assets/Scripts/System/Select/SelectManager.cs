using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectManager : MonoBehaviour
{
    public enum SeasonEnum
    {
        [InspectorName("èt")]
        Spring,
        [InspectorName("âƒ")]
        Summer,
        [InspectorName("èH")]
        Autumn,
        [InspectorName("ì~")]
        Winter
    }
    public static Dictionary<SeasonEnum, string> _seasonKeys =new()
    {
        {SeasonEnum.Spring, "Spring"},
        {SeasonEnum.Summer, "Summer"},
        {SeasonEnum.Autumn, "Autumn"},
        {SeasonEnum.Winter, "Winter"}
    };

    [SerializeField]
    Season[] _seasons;
    [SerializeField]
    bool _isDebug = false;
    [SerializeField,Header("élãG")]
    Dictionary<string,Season> _seasonDic;
    // Start is called before the first frame update
    void Start()
    {
       
        _seasonDic = new Dictionary<string, Season>();
        PlayerPrefs.SetInt(_seasonKeys[SeasonEnum.Spring],1);
        
        for (int i = 0;i < _seasons.Length || i < _seasonKeys.Count;++i)
        {
            
            _seasonDic.Add(_seasonKeys[(SeasonEnum)i],_seasons[i]);
            //PlayerPrefsÇ©ÇÁéÛÇØéÊÇ¡ÇΩÇËÅAÇÁÇ∂ÇŒÇÒÇæÇË
            if (PlayerPrefs.HasKey(_seasonKeys[(SeasonEnum)i]))
            {
                _seasons[i].IsActive = PlayerPrefs.GetInt(_seasonKeys[(SeasonEnum)i]) != 0;
            }
            _seasons[i].SetUp();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnApplicationQuit()
    {
#if UnityEditor
        //PlayerPrefs.DeleteAll();
#endif
    }
    private void OnGUI()
    {
        if(_isDebug)
        {
            for (int i = 0; i < _seasonDic.Count; ++i)
            {
                if (GUI.Button(new Rect(15, 100 + i * 40, 100, 30), _seasonKeys[(SeasonEnum)i]))
                {
                    PlayerPrefs.SetInt(_seasonKeys[(SeasonEnum)i], 1);
                    SceneManager.LoadScene("SelectScene");
                }

            }

            if (GUI.Button(new Rect(15, 300, 100, 50), "KeyReset"))
            {
                PlayerPrefs.DeleteAll();
                SceneManager.LoadScene("SelectScene");
            }


        }

    }
}
