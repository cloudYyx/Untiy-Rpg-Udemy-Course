using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// װ����λ
/// </summary>
public class UI_EquipmentSlot : UI_ItemSlot
{
    //װ������
    public EquipmentType slotType;

    /// <summary>
    /// Unity �ڼ��ؽű��������е�ֵ����ʱ���õĽ��ޱ༭���ĺ���
    /// </summary>
    private void OnValidate()
    {
        gameObject.name = "Equipment slot - " + slotType.ToString();
    }

    /// <summary>
    /// �����A���󣬰������ʱA������Ӧ���¼�
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.itemData == null)
        {
            return;
        }
        //as:��ǿ������ת����һ����,������Զ�����׳��쳣,�����ת�����ɹ�,�᷵��null
        Inventory.instance.UnequipItem(item.itemData as ItemData_Equipment);
        Inventory.instance.AddItem(item.itemData as ItemData_Equipment);

        ui.itemTooltip.HideToolTip();

        CleanUpSlot();
    }
}
