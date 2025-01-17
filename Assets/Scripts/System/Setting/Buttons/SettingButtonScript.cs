using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
/// <summary>
/// ���̃N���X�͐ݒ��ʂŃ{�^�������ۂɌp�������Ă�<br/>
/// �I�����ꂽ���̏�����SelectedButton��override���Ă�<br/>
/// Start�֐��łȂɂ������������Ƃ���StartButton�֐���override����<br/>
/// �L�q����ƂȂ񂩂��������ɍ�p�����<br/>
/// �悭�킩��˂���J�X�Ƃ����Ƃ��͒ѐA�܂�
/// </summary>
public class SettingButtonScript : MonoBehaviour
{
    protected RectTransform _rect;
    [SerializeField,Header("�g�厞��")]
    protected float _zoomInTime = 1.0f;
    [SerializeField,Header("�g�k����")]
    protected float _zoomOutTime = 1.0f;
    [SerializeField,Header("�g��{��")]
    protected float _zoomInRatio = 1.5f;
    public virtual void SelectedButton() { }

    private void Start()
    {
        _rect = GetComponent<RectTransform>();
        StartButton();
    }

    public void SetScale()
    {
        GetComponent<Outline>().enabled = true;
        _rect.DOKill();
        _rect.DOScale(new Vector3(_zoomInRatio,_zoomInRatio),_zoomInTime).SetUpdate(true);
    }

    protected virtual void StartButton()
    {

    }
    public void ResetScale()
    {
        GetComponent<Outline>().enabled = false;
        _rect.DOKill();
        _rect.DOScale(new Vector3(1f,1f), _zoomOutTime).SetUpdate(true);
    }
}
