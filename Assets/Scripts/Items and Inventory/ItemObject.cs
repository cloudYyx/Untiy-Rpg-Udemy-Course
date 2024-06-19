using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��Ʒ����
/// </summary>
public class ItemObject : MonoBehaviour
{
    //��Ʒ����
    [SerializeField] private ItemData itemData;
    //����
    [SerializeField] private Rigidbody2D rb;

    /// <summary>
    /// ���û���
    /// </summary>
    private void SetupVisuals()
    {
        if (itemData == null)
        {
            return;
        }

        //��Ⱦ2Dͼ��
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object - " + itemData.itemName;
    }

    /// <summary>
    /// ������Ʒ����
    /// </summary>
    /// <param name="_itemData">��Ʒ����</param>
    /// <param name="_velocity">�����ٶ�</param>
    public void SetupItem(ItemData _itemData,Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;

        SetupVisuals();
    }

    /// <summary>
    /// ʰȡ��Ʒ
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
