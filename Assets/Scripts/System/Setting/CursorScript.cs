using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
//‚¢‚Á‚½‚ñ•s–@“ŠŠü
public class CursorScript : MonoBehaviour
{
    RectTransform _rect;
    Vector2 _startSize;
    // Start is called before the first frame update
    void Start()
    {
        _rect = GetComponent<RectTransform>();
        _startSize = _rect.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveCursor(RectTransform _target)
    {
        //_rect.DOKill();
        _rect.DOMove(_target.position, 0.2f).SetUpdate(true);
        _rect.DOScale(1.5f,0.1f).SetUpdate(true);
    }
}
