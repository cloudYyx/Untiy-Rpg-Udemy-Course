using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// 物品槽位UI
/// IPointerDownHandler : 响应鼠标在自身碰撞体范围内按下事件
/// IPointerEnterHandler : 响应鼠标进入自身碰撞体范围事件
/// IPointerExitHandler ： 响应鼠标离开自身碰撞体范围事件
/// </summary>
public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
    //物品图像
    [SerializeField] protected Image itemImage;
    //物品文本（文本插件）
    [SerializeField] protected TextMeshProUGUI itemText;

    //用户页面
    protected UI ui;
    //库存物品
    public InventoryItem item;

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    /// <summary>
    /// 更新物品槽位
    /// </summary>
    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;
        //更新前颜色默认透明，更新后变为可见
        itemImage.color = Color.white;

        if (item != null)
        {
            itemImage.sprite = item.itemData.icon;

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    /// <summary>
    /// 清理物品槽位
    /// </summary>
    public void CleanUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    /// <summary>
    /// 响应鼠标在自身碰撞体范围内按下事件
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
        {
            return;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.itemData);
            return;
        }

        if (item.itemData.itemType == ItemType.Equipment) 
        {
            Inventory.instance.EquipItem(item.itemData);
        }

        ui.itemTooltip.HideToolTip();
    }

    /// <summary>
    /// 响应鼠标进入自身碰撞体范围事件
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
        {
            return;
        }

        //根据鼠标位置显示相应的技能描述
        Vector2 mousePosition = Input.mousePosition;

        float xOffset = 0;
        float yOffset = 0;

        if (mousePosition.x > 300)
        {
            xOffset = -150;
        }
        else
        {
            xOffset = 150;
        }

        if (mousePosition.y > 300)
        {
            yOffset = -150;
        }
        else
        {
            yOffset = 150;
        }

        ui.itemTooltip.ShowToolTip(item.itemData as ItemData_Equipment);
        ui.itemTooltip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }

    /// <summary>
    /// 响应鼠标离开自身碰撞体范围事件
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null)
        {
            return;
        }

        ui.itemTooltip.HideToolTip();
    }
}
