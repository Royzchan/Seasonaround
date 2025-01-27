using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : SettingButtonScript
{
    public override void SelectedButton()
    {
        FadeManager.Instance.LoadScene(SceneManager.GetActiveScene().name,0.2f);
    }
}
