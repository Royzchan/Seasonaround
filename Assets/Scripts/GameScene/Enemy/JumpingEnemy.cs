using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class JumpingEnemy : MonoBehaviour
{
    private Rigidbody _rb;

    GameObject player;

    [SerializeField, Header("�v���C���[�Ƃ̋���")]
    private float _distance = 5f;

    [SerializeField, Header("�W�����v��")]
    private float _jumpPower = 5.0f;

    //�W�����v�̉�
    private int _jumpNum = 0;

    [SerializeField, Header("�W�����v�ł����")]
    private int _canJumpNum = 1;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            float _dis = Vector3.Distance(this.transform.position, player.transform.position);
            Jump(_dis);
        }
    }

    void Jump(float dis)
    {
        if (dis <= _distance)
        {
            //�W�����v����
            if (_jumpNum < _canJumpNum)
            {
                _rb.AddForce(0f, _jumpPower, 0f, ForceMode.Impulse);
                _jumpNum++;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _jumpNum = 0;
        }
    }
}
