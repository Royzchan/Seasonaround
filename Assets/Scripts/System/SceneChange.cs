using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField]
    string _openingScene;
    [SerializeField]
    string _selectScene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScene()
    {
        if (!OpeningOneTime.Instance.hasWatchedOpening)
        {
            OpeningOneTime.Instance.hasWatchedOpening = true; // フラグを更新
            FadeManager.Instance.LoadScene(_openingScene, 1.0f);
        }
        else
        {
            FadeManager.Instance.LoadScene(_selectScene, 1.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FadeManager.Instance.LoadScene(_selectScene, 1.0f);
        }
    }
}
