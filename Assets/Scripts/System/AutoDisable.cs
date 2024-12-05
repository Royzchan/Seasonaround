using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisable : MonoBehaviour
{
    public float _disableTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator DisableObj()
    {
        yield return new WaitForSeconds(_disableTime);
        gameObject.SetActive(false);
    }
}
