using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// ��Ʒ��λUI
/// IPointerDownHandler : ��Ӧ�����������ײ�巶Χ�ڰ����¼�
/// IPointerEnterHandler : ��Ӧ������������ײ�巶Χ�¼�
/// IPointerExitHandler �� ��Ӧ����뿪������ײ�巶Χ�¼�
/// </summary>
public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
    //��Ʒͼ��
    [SerializeField] protected Image itemImage;
    //��Ʒ�ı����ı������
    [SerializeField] protected TextMeshProUGUI itemText;

    //�û�ҳ��
    protected UI ui;
    //�����Ʒ
    public InventoryItem item;

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    /// <summary>
    /// ������Ʒ��λ
    /// </summary>
    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;
        //����ǰ��ɫĬ��͸�������º��Ϊ�ɼ�
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
    /// ������Ʒ��λ
    /// </summary>
    public void CleanUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    /// <summary>
    /// ��Ӧ�����������ײ�巶Χ�ڰ����¼�
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
    /// ��Ӧ������������ײ�巶Χ�¼�
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
        {
            return;
        }

        //�������λ����ʾ��Ӧ�ļ�������
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
    /// ��Ӧ����뿪������ײ�巶Χ�¼�
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
