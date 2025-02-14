using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(WindScript))]
public class WindStopScript : MonoBehaviour,ISwitch
{
    bool _isStop = false;
    Vector3 _groundPos;
    // Start is called before the first frame update
    void Start()
    {
        Physics.Raycast(transform.position, transform.forward, out var hit);
        _groundPos = hit.point;
    }
    bool ISwitch.OnOffCheck() 
    { 
        return _isStop; 
    }
    // Update is called once per frame
    void Update()
    {
        
        
        Debug.DrawRay(_groundPos, -transform.forward,Color.yellow,0.3f);
        var raycasts = Physics.RaycastAll(_groundPos, -transform.forward, 0.3f);
        if (raycasts != null)
        {
            foreach (var hitInfo in raycasts)
            {
                if (hitInfo.transform.gameObject.CompareTag("Ground"))
                {
                    Debug.Log(hitInfo.transform.gameObject.name);
                    _isStop = true;
                    return;
                }
            }
            
        }
        _isStop = false;
    }
}
