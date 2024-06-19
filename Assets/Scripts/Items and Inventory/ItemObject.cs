using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 物品对象
/// </summary>
public class ItemObject : MonoBehaviour
{
    //物品数据
    [SerializeField] private ItemData itemData;
    //刚体
    [SerializeField] private Rigidbody2D rb;

    /// <summary>
    /// 设置画面
    /// </summary>
    private void SetupVisuals()
    {
        if (itemData == null)
        {
            return;
        }

        //渲染2D图形
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object - " + itemData.itemName;
    }

    /// <summary>
    /// 设置物品属性
    /// </summary>
    /// <param name="_itemData">物品数据</param>
    /// <param name="_velocity">刚体速度</param>
    public void SetupItem(ItemData _itemData,Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;

        SetupVisuals();
    }

    /// <summary>
    /// 拾取物品
    /// </summary>
    public void PickupItem()
    {
        if (!Inventory.instance.CanAddItem() && itemData.itemType == ItemType.Equipment)
        {
            rb.velocity = new Vector2(0, 7);
            return;
        }

        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
