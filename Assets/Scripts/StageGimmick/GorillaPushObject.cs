using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GorillaPushObject : MonoBehaviour
{
    Rigidbody _rb;
    PlayerController_2D _playerController;
    // Start is called before the first frame update
    void Start()
    {
        _playerController = FindAnyObjectByType<PlayerController_2D>();
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
     
       if (collision.gameObject.CompareTag("Player"))
       {
           if(_playerController.NowAnimal != Animal.Colobus)
           {
                //‰¡•ûŒü‚ÌˆÚ“®‚ðŽ~‚ß‚é
                _rb.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0); 
           }
       }
    }

}
