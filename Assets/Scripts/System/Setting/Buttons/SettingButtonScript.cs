using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
/// <summary>
/// このクラスは設定画面でボタンを作る際に継承させてね<br/>
/// 選択された時の処理はSelectedButtonをoverrideしてね<br/>
/// Start関数でなにか処理したいときはStartButton関数をoverrideして<br/>
/// 記述するとなんかいい感じに作用するよ<br/>
/// よくわかんねぇよカスというときは柘植まで
/// </summary>
public class SettingButtonScript : MonoBehaviour
{
    protected RectTransform _rect;
    [SerializeField,Header("拡大時間")]
    protected float _zoomInTime = 1.0f;
    [SerializeField,Header("拡縮時間")]
    protected float _zoomOutTime = 1.0f;
    [SerializeField,Header("拡大倍率")]
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
