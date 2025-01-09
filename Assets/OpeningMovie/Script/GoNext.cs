using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoNext : MonoBehaviour
{
    [SerializeField]
    string _sceneName;
    public void GoNextMovie()
    {
        FadeManager.Instance.LoadScene(_sceneName,1.0f);
    }
}
