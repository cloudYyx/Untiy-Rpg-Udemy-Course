using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// 技能工具提示UI
/// </summary>
public class UI_SkillToolTip : MonoBehaviour
{
    //技能文本
    [SerializeField] private TextMeshProUGUI skillText;
    //技能名字
    [SerializeField] private TextMeshProUGUI skillName; 

    /// <summary>
    /// 显示工具提示
    /// </summary>
    public void ShowToolTip(string _skillDescprtion,string _skillName)
    {
        skillText.text = _skillDescprtion;
        skillName.text = _skillName;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 隐藏工具提示
    /// </summary>
    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }
}
