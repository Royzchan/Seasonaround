using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaUIScript : MonoBehaviour
{
    [SerializeField,Header("ÉQÅ[ÉW")]
    Slider gauge;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GaugeUpdate(float now,float max)
    {
        float ratio = now / max;
        gauge.value = ratio;
        gauge.fillRect.GetComponent<Image>().color = new Color(1,ratio,0);
    }
}
