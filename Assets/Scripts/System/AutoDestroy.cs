using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float _time = 1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private  IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(_time);
        Destroy(gameObject);
    }
}
