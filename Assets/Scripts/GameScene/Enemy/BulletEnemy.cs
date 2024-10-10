using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    GameObject player;

    [SerializeField, Header("�v���C���[�Ƃ̋���")]
    private float _distance = 5f;

    [SerializeField, Header("�e�̃I�u�W�F�N�g")]
    private GameObject bulletPrefab;

    [SerializeField, Header("�e�̐����ꏊ")]
    private Transform bulletPosition;

    [SerializeField, Header("�����Ԋu")]
    private float _repeatSpan = 3f;

    // �o�ߎ���
    private float _timeElapsed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        _timeElapsed = 3;   //�o�ߎ��Ԃ����Z�b�g
    }

    // Update is called once per frame
    void Update()
    {
        _timeElapsed += Time.deltaTime;  // ���Ԃ��J�E���g����

        if (player != null)
        {
            // �v���C���[�ƓG�̋�������p
            float _dis = Vector3.Distance(this.transform.position, player.transform.position);

            // �v���C���[�ƓG�̈ʒu�֌W�p
            float _compare = this.transform.position.x - player.transform.position.x;

            LeftorRight(_compare);
            Shoot(_dis);
        }
    }

    // ����(�e�̃v���n�u�����p)
    void Shoot(float dis)
    {
        if (dis <= _distance)
        {
            if (_timeElapsed >= _repeatSpan)
            {
                // �e�̐���
                GameObject bullet = Instantiate(bulletPrefab, bulletPosition.position, bulletPosition.rotation);

                _timeElapsed = 0;
            }
        }
    }

    // ���������E������
    void LeftorRight(float compare)
    {
        if (compare > 0)
        {
            this.transform.eulerAngles = new Vector3(0, -90, 0);
        }
        else
        {
            this.transform.eulerAngles = new Vector3(0, 90, 0);
        }
    }
}
