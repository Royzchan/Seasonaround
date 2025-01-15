using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    CameraFollow2D _cameraFollow2D;

    [SerializeField,Header("ÉYÅ[ÉÄíl")]
    float _zoomValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        _cameraFollow2D = Camera.main.GetComponent<CameraFollow2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _cameraFollow2D.Zoom(_zoomValue);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _cameraFollow2D.ZoomOut();
        }
    }
}
