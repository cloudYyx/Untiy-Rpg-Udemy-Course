using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
/// <summary>
/// 工艺列表UI
/// IPointerDownHandler : 响应鼠标在自身碰撞体范围内按下事件
/// </summary>
public class UI_CraftList : MonoBehaviour,IPointerDownHandler
{
    //工艺槽(父节点)
    [SerializeField] private Transform craftSlotParent;
    //工艺槽预制体
    [SerializeField] private GameObject craftSlotPrefab;

    //工艺装备
    [SerializeField] private List<ItemData_Equipment> craftEquipment;


    void Start()
    {
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetupCraftList();
        SetupDefaultCraftWindow();
    }

    /// <summary>
    /// 设置工艺列表属性
    /// </summary>
    public void SetupCraftList()
    {
        //删除所有工艺槽
        for (int i = 0; i < craftSlotParent.childCount; i++) 
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < craftEquipment.Count; i++)
        {
            //craftSlotPrefab	要复制的现有对象。craftSlotParent 新对象的位置。
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<UI_CraftSlot>().SetupCraftSlot(craftEquipment[i]);
        }
    }

    /// <summary>
    /// 响应鼠标在自身碰撞体范围内按下事件
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();
    }

    /// <summary>
    /// 设置默认工艺窗口
    /// </summary>
    public void SetupDefaultCraftWindow()
    {
        if (craftEquipment[0] != null)
        {
            GetComponentInParent<UI>().craftWindow.SetupCraftWindow(craftEquipment[0]);
        }
    }
}
