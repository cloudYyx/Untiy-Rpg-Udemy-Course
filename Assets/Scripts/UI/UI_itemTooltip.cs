using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// ��Ʒ������ʾUI
/// </summary>
public class UI_itemTooltip : MonoBehaviour
{
    //��Ʒ�����ı�
    [SerializeField] private TextMeshProUGUI itemNameText;
    //��Ʒ�����ı�
    [SerializeField] private TextMeshProUGUI itemTypeText;
    //��Ʒ�����ı�
    [SerializeField] private TextMeshProUGUI itemDescription;

    //Ĭ�������С
    [SerializeField] private int defaultFontSize = 32;

    /// <summary>
    /// ��ʾ������ʾ
    /// </summary>
    /// <param name="item"></param>
    public void ShowToolTip(ItemData_Equipment item)
    {
        if (item == null)
        {
            return;
        }

        itemNameText.text = item.itemName;
        itemTypeText.text = item.equipmentType.ToString();
        itemDescription.text = item.GetDescription();

        if (itemNameText.text.Length > 12)
        {
            itemNameText.fontSize = defaultFontSize * 0.7f;
        }
        else
        {
            itemNameText.fontSize = defaultFontSize;
        }

        gameObject.SetActive(true);
    }

    /// <summary>
    /// ���ع�����ʾ
    /// </summary>
    public void HideToolTip()
    {
        if (itemNameText.text.Length > 12)
        {
            itemNameText.fontSize = defaultFontSize;
        }
        gameObject.SetActive(false);
    }
}
