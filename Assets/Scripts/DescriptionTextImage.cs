using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionTextImage : MonoBehaviour
{
    Text _descriptionText;

    Image _image;

    // Start is called before the first frame update
    void Start()
    {
        _descriptionText = GameObject.Find("DescriptionText").GetComponent<Text>();
        _image = GetComponent<Image>();
        _image.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_descriptionText.enabled)
        {
            _image.enabled = true;
        }
        else
        {
            _image.enabled = false;
        }
    }
}
