using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// ���ղ�
/// </summary>
public class UI_CraftSlot : UI_ItemSlot
{
    //Ĭ�������С
    [SerializeField] private int defaultFontSize = 24;

    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// ���ù��ղ�����
    /// </summary>
    /// <param name="_data"></param>
    public void SetupCraftSlot(ItemData_Equipment _data)
    {
        if (_data == null)
        {
            return;
        }

        item.itemData = _data;

        itemImage.sprite = _data.icon;
        itemText.text = _data.itemName;

        if (itemText.text.Length > 12)
        {
            itemText.fontSize = defaultFontSize * 0.7f;
        }
        else
        {
            itemText.fontSize = defaultFontSize;
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
        {
            return;
        }

        ui.craftWindow.SetupCraftWindow(item.itemData as ItemData_Equipment);
    }
}
