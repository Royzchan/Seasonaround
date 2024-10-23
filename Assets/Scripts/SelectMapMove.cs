using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMapMove : MonoBehaviour
{
    public float nowPosi;
    // Start is called before the first frame update
    void Start()
    {
        nowPosi = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, nowPosi + Mathf.PingPong(Time.time/3, 0.3f), transform.position.z);
    }
}
