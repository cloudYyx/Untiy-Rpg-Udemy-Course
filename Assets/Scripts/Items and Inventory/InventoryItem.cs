using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 储存室物品
/// </summary>
[Serializable]
public class InventoryItem
{
    //物品数据
    public ItemData itemData;
    //库存尺寸
    public int stackSize;

    public InventoryItem(ItemData _itemData)
    {
        itemData = _itemData;
        AddStack();
    }

    /// <summary>
    /// 尺寸增加
    /// </summary>
    public void AddStack()
    {
        stackSize++;
    }

    /// <summary>
    /// 尺寸减少
    /// </summary>
    public void RemoveStack()
    {
        stackSize--;
    }
}
