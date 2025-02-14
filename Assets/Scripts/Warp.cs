using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Warp : MonoBehaviour
{
    Quaternion _firstRotation;

    private void Start()
    {
        _firstRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
