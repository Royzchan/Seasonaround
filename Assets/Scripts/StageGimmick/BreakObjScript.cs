using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BreakObjScript : MonoBehaviour
{
    [SerializeField, Header("“§–¾(Á–Å)‚É‚È‚é‚Ü‚Å‚ÌŠÔ")]
    float _fadeTime;
    [SerializeField,Header("‚«”ò‚Ô¨‚¢")]
    int _burstPower = 3;
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
        var random = new System.Random();
        var min = -_burstPower;
        var max = _burstPower;
        gameObject.GetComponentsInChildren<Rigidbody>().ToList().ForEach(r => {
            r.isKinematic = false;
            r.transform.SetParent(null);
            //«¡“x‚¢‚ë‚¢‚ë‚µ‚Ä‚©‚çÀ‘•‚·‚é
            //if(r.gameObject.TryGetComponent(out MeshRenderer a))
            //{
            //    a.material.DOFade(0f, _fadeTime);
            //}
            r.gameObject.AddComponent<AutoDestroy>()._time = _fadeTime;
            var vect = new Vector3(random.Next(min, max), random.Next(0, max), random.Next(min, max));
            r.AddForce(vect, ForceMode.Impulse);
            r.AddTorque(vect, ForceMode.Impulse);
        });
        Destroy(gameObject);
    }
}

