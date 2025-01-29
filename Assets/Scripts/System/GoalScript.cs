using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

public class GoalScript : MonoBehaviour
{
    PlayerController_2D _player;
    [SerializeField, Header("�ߊ�鋗��")]
    Vector3 _cameraDistance = new Vector3(0f,2f,-2f);
    [SerializeField, Header("�J�����̈ړ�����")]
    float _cameraMoveTime = 1f;
    [SerializeField, Header("�e�L�X�g")]
    GameObject _clearText;
    [SerializeField, Header("�e�L�X�g�̊g��̎���")]
    float _textZoomTime = 1f;
    [SerializeField, Header("�V�[���J�ڃA�j���[�V����")]
    RectTransform _transition;
    [SerializeField,Header("�ڍs��̃V�[��")]
    string _sceneName;
    CameraFollow2D _cameraScript;
    [SerializeField, Header("PlayerPrefs��Key")]
    string _seasonKey;
    private void Start()
    {
        _player = FindAnyObjectByType<PlayerController_2D>();
        _cameraScript = FindAnyObjectByType<CameraFollow2D>();
        _clearText.transform.localScale = Vector3.zero;
        _transition.parent.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Goal()
    {
        //�Ǐ]��؂�
        _cameraScript.enabled = false;
        //�v���C���[�̈ʒu���擾
        Transform playerPos = _player.transform;
        //�v���C���[�͑���s�\
        _player.enabled = false;
        //���n��҂�
        yield return new WaitForSeconds(0.8f);
        //�J�������v���C���[�̈ʒu�Ɉړ�
        Camera.main.transform.DOMove(playerPos.position + _cameraDistance,_cameraMoveTime);
        yield return new WaitForSeconds(_cameraMoveTime + 0.5f);
        _clearText.gameObject.SetActive(true);
        //�g��
        _clearText.transform.DOScale(1f,_textZoomTime - _textZoomTime * 0.2f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(_textZoomTime);
        _transition.parent.gameObject.SetActive(true);
        //Mask�̍X�V�̊֌W�ň�u��ʂ��^���Âɂ�������߁A�����ɂ��Ĉ�u�ҋ@��,�߂�
        _transition.parent.GetComponent<Image>().color -= new Color(0,0,0,255); 
        yield return new WaitForEndOfFrame();
        _transition.parent.GetComponent<Image>().color += new Color(0, 0, 0, 255);
        //�J�ڗp�摜��True�ɂ��Ċg��
        //.gameObject.SetActive(true);
        _transition.DOSizeDelta(new Vector2(0,0), 1f);
        yield return new WaitForSeconds(1.5f);
        FadeManager.Instance.LoadScene(_sceneName,0.2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var restart = FindAnyObjectByType<RestartManager>();
            if (restart != null)
            {
                //�I�����Ƀ��X�|�[���n�_�̃��Z�b�g
                restart.ResetRestartPos();
            }
            PlayerPrefs.SetInt(_seasonKey,0);
            StartCoroutine(Goal());
        }
    }
}
