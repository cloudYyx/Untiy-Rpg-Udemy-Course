using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// ���ܹ�����ʾUI
/// </summary>
public class UI_SkillToolTip : MonoBehaviour
{
    //�����ı�
    [SerializeField] private TextMeshProUGUI skillText;
    //��������
    [SerializeField] private TextMeshProUGUI skillName; 

    /// <summary>
    /// ��ʾ������ʾ
    /// </summary>
    public void ShowToolTip(string _skillDescprtion,string _skillName)
    {
        skillText.text = _skillDescprtion;
        skillName.text = _skillName;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// ���ع�����ʾ
    /// </summary>
    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }
}
