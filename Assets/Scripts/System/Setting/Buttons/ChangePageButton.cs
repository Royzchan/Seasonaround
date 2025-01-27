using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  ChangePageButton : SettingButtonScript
{
    [SerializeField, Header("ポーズ用のページ")]
    SettingPage _nowPage;
    [SerializeField,Header("オプション用のページ")]
    SettingPage _nextPage;

    public override void SelectedButton()
    {
        //開いているページ番号に移動
        SettingManager.GetInstance().ChangePage(_nextPage._pageNum);
    }
}

