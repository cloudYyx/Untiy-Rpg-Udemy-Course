using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
/// <summary>
/// �����б�UI
/// IPointerDownHandler : ��Ӧ�����������ײ�巶Χ�ڰ����¼�
/// </summary>
public class UI_CraftList : MonoBehaviour,IPointerDownHandler
{
    //���ղ�(���ڵ�)
    [SerializeField] private Transform craftSlotParent;
    //���ղ�Ԥ����
    [SerializeField] private GameObject craftSlotPrefab;

    //����װ��
    [SerializeField] private List<ItemData_Equipment> craftEquipment;


    void Start()
    {
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetupCraftList();
        SetupDefaultCraftWindow();
    }

    /// <summary>
    /// ���ù����б�����
    /// </summary>
    public void SetupCraftList()
    {
        //ɾ�����й��ղ�
        for (int i = 0; i < craftSlotParent.childCount; i++) 
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < craftEquipment.Count; i++)
        {
            //craftSlotPrefab	Ҫ���Ƶ����ж���craftSlotParent �¶����λ�á�
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<UI_CraftSlot>().SetupCraftSlot(craftEquipment[i]);
        }
    }

    /// <summary>
    /// ��Ӧ�����������ײ�巶Χ�ڰ����¼�
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();
    }

    /// <summary>
    /// ����Ĭ�Ϲ��մ���
    /// </summary>
    public void SetupDefaultCraftWindow()
    {
        if (craftEquipment[0] != null)
        {
            GetComponentInParent<UI>().craftWindow.SetupCraftWindow(craftEquipment[0]);
        }
    }
}
