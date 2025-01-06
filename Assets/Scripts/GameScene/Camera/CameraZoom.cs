using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    CameraFollow2D _cameraFollow2D;

    [SerializeField,Header("ズーム値")]
    float _zoomValue = 0;
    [SerializeField,Header("プレイヤーにズームするか")]
    bool _isPlayerZoom = true;
    [SerializeField, Header("プレイヤー以外にズームするときのポジション")]
    Transform _zoomObj;

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
            if (_isPlayerZoom)
            {
                _cameraFollow2D.Zoom(_zoomValue);
            }
            else
            {
                _cameraFollow2D.ZoomObj(_zoomValue, _zoomObj.position, 5f);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_isPlayerZoom) _cameraFollow2D.ZoomOut();
        }
    }
}
