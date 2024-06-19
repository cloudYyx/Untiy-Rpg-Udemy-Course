using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// 装备槽位
/// </summary>
public class UI_EquipmentSlot : UI_ItemSlot
{
    //装备类型
    public EquipmentType slotType;

    /// <summary>
    /// Unity 在加载脚本或检查器中的值更改时调用的仅限编辑器的函数
    /// </summary>
    private void OnValidate()
    {
        gameObject.name = "Equipment slot - " + slotType.ToString();
    }

    /// <summary>
    /// 鼠标点击A对象，按下鼠标时A对象响应此事件
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.itemData == null)
        {
            return;
        }
        //as:与强制类型转换是一样的,但是永远不会抛出异常,即如果转换不成功,会返回null
        Inventory.instance.UnequipItem(item.itemData as ItemData_Equipment);
        Inventory.instance.AddItem(item.itemData as ItemData_Equipment);

        ui.itemTooltip.HideToolTip();

        CleanUpSlot();
    }
}
