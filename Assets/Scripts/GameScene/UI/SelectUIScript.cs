using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class SelectUIScript : MonoBehaviour
{
    [SerializeField, Header("è„â∫ç∂âEÇÃImage 0:Å™,1:Å®,2:Å´,3:Å©")]
    Image[] _images;
    [SerializeField, Header("ìÆï®ÇÃÉÇÉfÉã")]
    GameObject[] _animals;
    bool[] _canSelect = {false,false,false,false};
    int _selectNum = 0;
    [SerializeField]
    float _cameraSize = 110;
    [SerializeField]
    Camera _uiCamera;
    // Start is called before the first frame update
    private void Awake()
    {
        foreach (var item in _animals)
        {
            item.SetActive(false);
        }
        gameObject.SetActive(false);
        _uiCamera.orthographicSize = _cameraSize * transform.parent.localScale.y;

    }

    private void OnEnable()
    {
        _uiCamera.orthographicSize = _cameraSize * transform.parent.localScale.y;
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetHitAnimal(int num)
    {
        _canSelect[num] = true;
        _animals[num].SetActive(true);
        _images[num].color = Color.white;
    }
    public void ScaleMove(int num)
    {
        if (!_canSelect[num]) return;
        foreach (var image in _images)
        {
            image.rectTransform.DOComplete();
        }
        _images[_selectNum].rectTransform.DOScale(Vector3.one,0.05f);
        _images[num].rectTransform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.1f);
        _selectNum = num;
    }
    public void ResetScale()
    {
        foreach (var image in _images)
        {
            image.rectTransform.DOComplete();
            image.rectTransform.localScale = Vector3.one;

        }

    }
}
