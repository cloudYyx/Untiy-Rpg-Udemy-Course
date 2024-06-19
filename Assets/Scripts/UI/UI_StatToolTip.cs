using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// ͳ�ƹ�����ʾUI
/// </summary>
public class UI_StatToolTip : MonoBehaviour
{
    //�����ı�
    [SerializeField] private TextMeshProUGUI description;

    /// <summary>
    /// ��ʾ������ʾ
    /// </summary>
    /// <param name="_text"></param>
    public void ShowStatToolTip(string _text)
    {
        description.text = _text;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// ���ع�����ʾ
    /// </summary>
    public void HideStatToolTip()
    {
        description.text = "";
        gameObject.SetActive(false);
    }
}
