using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionTextCollider : MonoBehaviour
{
    Text _descriptionText;

    [SerializeField, TextArea]
    string _text;

    // Start is called before the first frame update
    void Start()
    {
        _descriptionText = GameObject.Find("DescriptionText").GetComponent<Text>();
        _descriptionText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _descriptionText.text = _text;
            _descriptionText.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _descriptionText.enabled = false;
        }
    }

    public void Enable(float _time)
    {
        _descriptionText.text = _text;
        _descriptionText.enabled = true;

        Invoke("UnEnable", _time);
    }

    void UnEnable()
    {
        _descriptionText.enabled = false;
    }
}
