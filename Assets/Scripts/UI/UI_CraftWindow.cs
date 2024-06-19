using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ���մ���UI
/// </summary>
public class UI_CraftWindow : MonoBehaviour
{
    //��Ʒ����
    [SerializeField] private TextMeshProUGUI itemName;
    //��Ʒ����
    [SerializeField] private TextMeshProUGUI itemDescription;
    //��ƷͼƬ
    [SerializeField] private Image itemIcon;
    //����һ��������ť�����û�����ð�ťʱ������ִ��һЩ������
    [SerializeField] private Button craftButton;

    //����ͼƬ
    [SerializeField] private Image[] materialImage;

    /// <summary>
    /// ���ù��մ���
    /// </summary>
    /// <param name="_itemData_Equipment"></param>
    public void SetupCraftWindow(ItemData_Equipment _itemData_Equipment)
    {
        //���¼���ɾ�����зǳ־��ԣ����ӽű������ģ���������
        craftButton.onClick.RemoveAllListeners();

        //�������ͼƬ
        for (int i = 0; i < materialImage.Length; i++)
        {
            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }
        //���ò���ͼƬ������
        for (int i = 0; i < _itemData_Equipment.craftingMaterials.Count; i++)
        {
            if (_itemData_Equipment.craftingMaterials.Count > materialImage.Length)
            {
                Debug.LogWarning("��Ĳ����������������ڹ��մ����еĲ��ϲ�");
            }

            materialImage[i].sprite = _itemData_Equipment.craftingMaterials[i].itemData.icon;
            materialImage[i].color = Color.white;

            TextMeshProUGUI materialSlotText = materialImage[i].GetComponentInChildren<TextMeshProUGUI>();

            materialSlotText.text = _itemData_Equipment.craftingMaterials[i].stackSize.ToString();            
            materialSlotText.color = Color.white;
        }
        //����װ��������Ʒ��ͼƬ�����֣�����
        itemIcon.sprite = _itemData_Equipment.icon;
        itemName.text = _itemData_Equipment.itemName;
        itemDescription.text = _itemData_Equipment.GetDescription();
        //��Ӽ�����
        craftButton.onClick.AddListener(
            () => Inventory.instance.CanCraft(_itemData_Equipment, _itemData_Equipment.craftingMaterials));
    }
}
