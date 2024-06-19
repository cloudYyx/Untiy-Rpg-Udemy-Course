using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 工艺窗口UI
/// </summary>
public class UI_CraftWindow : MonoBehaviour
{
    //物品名称
    [SerializeField] private TextMeshProUGUI itemName;
    //物品描述
    [SerializeField] private TextMeshProUGUI itemDescription;
    //物品图片
    [SerializeField] private Image itemIcon;
    //创建一个单击按钮。当用户点击该按钮时，立即执行一些操作。
    [SerializeField] private Button craftButton;

    //材料图片
    [SerializeField] private Image[] materialImage;

    /// <summary>
    /// 设置工艺窗口
    /// </summary>
    /// <param name="_itemData_Equipment"></param>
    public void SetupCraftWindow(ItemData_Equipment _itemData_Equipment)
    {
        //从事件中删除所有非持久性（即从脚本创建的）侦听器。
        craftButton.onClick.RemoveAllListeners();

        //清楚材料图片
        for (int i = 0; i < materialImage.Length; i++)
        {
            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }
        //设置材料图片和数量
        for (int i = 0; i < _itemData_Equipment.craftingMaterials.Count; i++)
        {
            if (_itemData_Equipment.craftingMaterials.Count > materialImage.Length)
            {
                Debug.LogWarning("你的材料数量超过了你在工艺窗口中的材料槽");
            }

            materialImage[i].sprite = _itemData_Equipment.craftingMaterials[i].itemData.icon;
            materialImage[i].color = Color.white;

            TextMeshProUGUI materialSlotText = materialImage[i].GetComponentInChildren<TextMeshProUGUI>();

            materialSlotText.text = _itemData_Equipment.craftingMaterials[i].stackSize.ToString();            
            materialSlotText.color = Color.white;
        }
        //设置装备或者物品的图片，名字，描述
        itemIcon.sprite = _itemData_Equipment.icon;
        itemName.text = _itemData_Equipment.itemName;
        itemDescription.text = _itemData_Equipment.GetDescription();
        //添加监听器
        craftButton.onClick.AddListener(
            () => Inventory.instance.CanCraft(_itemData_Equipment, _itemData_Equipment.craftingMaterials));
    }
}
