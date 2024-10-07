using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField, Header("�e�̑��x")]
    private float _bulletSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        // �e��O���Ɉړ�������
        transform.Translate(Vector3.forward * _bulletSpeed * Time.deltaTime);

        Destroy(this.gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �ǂ�v���C���[�ɓ���������e������
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);  // �e���폜
        }
    }
}
