using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectManager : MonoBehaviour
{
    [SerializeField]
    string[] _seasonNames;
    [SerializeField]
    Season[] _seasons;
    [SerializeField]
    bool _isDebug = false;
    [SerializeField,Header("Žl‹G")]
    Dictionary<string,Season> _seasonDic;
    // Start is called before the first frame update
    void Start()
    {
        _seasonDic = new Dictionary<string, Season>();
        PlayerPrefs.SetInt(_seasonNames[0], 1);

        for (int i = 0;i < _seasons.Length || i < _seasonNames.Length;++i)
        {
            _seasonDic.Add(_seasonNames[i],_seasons[i]);
            //PlayerPrefs‚©‚çŽó‚¯Žæ‚Á‚½‚èA‚ç‚¶‚Î‚ñ‚¾‚è
            if (PlayerPrefs.HasKey(_seasonNames[i]))
            {
                _seasons[i].IsActive = PlayerPrefs.GetInt(_seasonNames[i]) != 0;
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
                if (GUI.Button(new Rect(15, 100 + i * 40, 100, 30), _seasonNames[i]))
                {
                    PlayerPrefs.SetInt(_seasonNames[i], 1);
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
