using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : SettingButtonScript
{
    
    public override void SelectedButton()
    {
        FadeManager.Instance.LoadScene("TitleScene", 0.5f);
       
    }
}
