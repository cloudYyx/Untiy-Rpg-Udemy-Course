using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��������Ʒ
/// </summary>
[Serializable]
public class InventoryItem
{
    //��Ʒ����
    public ItemData itemData;
    //���ߴ�
    public int stackSize;

    public InventoryItem(ItemData _itemData)
    {
        itemData = _itemData;
        AddStack();
    }

    /// <summary>
    /// �ߴ�����
    /// </summary>
    public void AddStack()
    {
        stackSize++;
    }

    /// <summary>
    /// �ߴ����
    /// </summary>
    public void RemoveStack()
    {
        stackSize--;
    }
}
