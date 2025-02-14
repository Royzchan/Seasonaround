using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipSlopeController : MonoBehaviour
{
    [SerializeField, Header("ŠŠ‚é—Í")]
    private Vector3 _slipPower = new Vector3(0, 0, 0);

    private Rigidbody _playerRB;

    private void Start()
    {
        _playerRB = FindAnyObjectByType<PlayerController_2D>()
            .gameObject.GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_playerRB != null)
            {
                _playerRB.velocity = _slipPower;
            }
        }
    }
}
