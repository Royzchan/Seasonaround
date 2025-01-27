using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnButton : SettingButtonScript
{

    public override void SelectedButton() 
    { 
        StartCoroutine(SettingManager.GetInstance().CloseSetting());
    }
}
