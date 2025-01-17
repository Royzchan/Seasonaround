using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPage : MonoBehaviour
{
    [SerializeField, Header("ページ内のボタン")]
    public SettingButtonScript[] _buttons;
    public int _pageNum;
}
