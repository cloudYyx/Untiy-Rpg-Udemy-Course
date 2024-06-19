using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// 统计工具提示UI
/// </summary>
public class UI_StatToolTip : MonoBehaviour
{
    //描述文本
    [SerializeField] private TextMeshProUGUI description;

    /// <summary>
    /// 显示工具提示
    /// </summary>
    /// <param name="_text"></param>
    public void ShowStatToolTip(string _text)
    {
        description.text = _text;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 隐藏工具提示
    /// </summary>
    public void HideStatToolTip()
    {
        description.text = "";
        gameObject.SetActive(false);
    }
}
