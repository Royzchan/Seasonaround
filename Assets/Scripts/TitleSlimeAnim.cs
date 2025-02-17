using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSlimeAnim : MonoBehaviour
{
    [SerializeField] GameObject[] _slimes;
    [SerializeField] GameObject[] _slimePoses;
    [SerializeField] float _animCoolTime;
    float _time = 0;
    bool _isAnim = false;
    Animator _anim;
    // Start is called before the first frame update
    void Start()
    {
        EndAnim();
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        if(_time > _animCoolTime)
        {
            if (_isAnim)
            {
                if (_anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                {
                    Debug.Log("アニメーション再生中");
                }
                else
                {
                    EndAnim();
                }
            }
            else
            {
                GameObject _slime = _slimes[Random.Range(0, _slimes.Length)];
                GameObject _slimePos = _slimePoses[Random.Range(0, _slimePoses.Length)];
                _slime.transform.position = _slimePos.transform.position;
                _slime.transform.rotation = _slimePos.transform.rotation;
                _slime.SetActive(true);
                _anim = _slime.GetComponent<Animator>();
                int rand = Random.Range(0, 2);
                if (rand == 0)
                {
                    _anim.SetTrigger("isRight");
                }
                else
                {
                    _anim.SetTrigger("isLeft");
                }
                _isAnim = true;
            }
        }
    }

    void EndAnim()
    {
        foreach (var slime in _slimes)
        {
            slime.SetActive(false);
        }
        _time = 0;
        _isAnim = false;
    }
}
