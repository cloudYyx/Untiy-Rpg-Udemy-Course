using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// 物品工具提示UI
/// </summary>
public class UI_itemTooltip : MonoBehaviour
{
    //物品名称文本
    [SerializeField] private TextMeshProUGUI itemNameText;
    //物品类型文本
    [SerializeField] private TextMeshProUGUI itemTypeText;
    //物品描述文本
    [SerializeField] private TextMeshProUGUI itemDescription;

    //默认字体大小
    [SerializeField] private int defaultFontSize = 32;

    /// <summary>
    /// 显示工具提示
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
    /// 隐藏工具提示
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
