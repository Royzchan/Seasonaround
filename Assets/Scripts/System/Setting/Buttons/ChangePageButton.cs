using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  ChangePageButton : SettingButtonScript
{
    [SerializeField, Header("�|�[�Y�p�̃y�[�W")]
    SettingPage _nowPage;
    [SerializeField,Header("�I�v�V�����p�̃y�[�W")]
    SettingPage _nextPage;

    public override void SelectedButton()
    {
        //�J���Ă���y�[�W�ԍ��Ɉړ�
        SettingManager.GetInstance().ChangePage(_nextPage._pageNum);
    }
}

