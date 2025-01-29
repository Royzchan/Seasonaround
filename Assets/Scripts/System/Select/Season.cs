using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Season : MonoBehaviour
{
    protected SceneChange _sc;
    protected bool _isActive = false;
    public bool IsActive { get { return _isActive; } set { _isActive = value; } }
    public void SetUp()
    {
        _sc = GetComponent<SceneChange>();
        if(!_isActive)
        {
            GetComponent<Collider>().isTrigger = false;
            _sc.enabled = false;
            foreach (var it in transform.GetComponentsInChildren<Renderer>())
            {
                it.material.color -= Color.gray * 1.5f;

            }
        }
    }
}
