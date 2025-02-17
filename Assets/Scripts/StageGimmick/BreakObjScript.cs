using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BreakObjScript : MonoBehaviour
{
    [SerializeField, Header("透明(消滅)になるまでの時間")]
    float _fadeTime;
    [SerializeField,Header("吹き飛ぶ勢い")]
    float _burstPower = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
            DestroyObject();
        }
    }
    public void DestroyObject()
    {

        
        var min = -_burstPower;
        var max = _burstPower;
        gameObject.GetComponentsInChildren<Rigidbody>().ToList().ForEach(r => {
            r.isKinematic = false;
            r.transform.SetParent(null);
            //↓今度いろいろ試してから実装する
            //if(r.gameObject.TryGetComponent(out MeshRenderer a))
            //{
            //    a.material.DOFade(0f, _fadeTime);
            //}
            r.gameObject.AddComponent<AutoDestroy>()._time = _fadeTime;
            r.gameObject.GetComponent<Collider>().enabled = false;
            var vect = new Vector3(Random.Range(min, max), Random.Range(0, max), Random.Range(min, max));
            r.AddForce(vect, ForceMode.Impulse);
            r.AddTorque(vect, ForceMode.Impulse);
        });
        Destroy(gameObject);
    }
}

