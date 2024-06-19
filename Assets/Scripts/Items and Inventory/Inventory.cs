using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
/// <summary>
/// ���
/// </summary>
public class Inventory : MonoBehaviour
{
    //ʵ��
    public static Inventory instance;

    //��ʼ��Ʒ
    public List<ItemData> startingItems;

    //װ��
    public List<InventoryItem> equipment;
    //�ֵ���keyֻ�ܶ�Ӧһ��ֵ���ܶ�Ӧ���ֵ�����Խṹ�� �൱��JAVA��MAP
    //װ�����ֵ�
    //ʵ������Dictionary<��key, ֵvalue> ����dic = new Dictionary<��key, ֵvalue>����;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    //���
    public List<InventoryItem> inventory;
    public Dictionary<ItemData,InventoryItem> inventoryDictionary;

    //������
    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    //���UI
    [Header("Inventory UI")]
    //���ۣ����ڵ㣬�������е���Ʒ�ۣ�������
    [SerializeField] private Transform inventorySlotParent;
    //�����Ҳۣ����ڵ�,�ֿ⣩
    [SerializeField] private Transform stashSlotParent;
    //װ����(���ڵ�)
    [SerializeField] private Transform equpmenntSlotParent;
    //ͳ�Ʋ�(���ڵ�)
    [SerializeField] private Transform statSlotParent;

    //�����Ʒ��λ
    private UI_ItemSlot[] inventoryItemSlots;
    //��������Ʒ��λ
    private UI_ItemSlot[] stashItemSlots;
    //װ����λ
    private UI_EquipmentSlot[] equipmentSlots;
    //ͳ�Ʋ�
    private UI_StatSlot[] statSlots;

    //��Ʒ��ȴ
    [Header("Item cooldown")]
    //�ϴ�ʹ��ƿ��ʱ��
    private float lastTimeUsedFlask;
    //�ϴ�ʹ������ʱ��
    private float lastTimeUsedArmor;

    //ƿ����ȴ
    private float flaskCooldown;
    //������ȴ
    private float armorCooldown;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        //GetComponents��ȡ�������������
        inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlots = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlots = equpmenntSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        statSlots = statSlotParent.GetComponentsInChildren<UI_StatSlot>();

        AddStartingItems();
    }

    /// <summary>
    /// �����ʼ��Ʒ
    /// </summary>
    private void AddStartingItems()
    {
        for (int i = 0; i < startingItems.Count; i++)
        {
            if (startingItems[i] != null)
            {
                AddItem(startingItems[i]);
            }
        }
    }

    /// <summary>
    /// װ����Ʒ
    /// </summary>
    /// <param name="_item"></param>
    public void EquipItem(ItemData _item)
    {
        //as:��ǿ������ת����һ����,������Զ�����׳��쳣,�����ת�����ɹ�,�᷵��null
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null;

        //1��KeyValuePair 
        //a��KeyValuePair ��һ���ṹ�壨struct����
        //b��KeyValuePair ֻ����һ��Key��Value�ļ�ֵ��
        // �����ֵ��������ֵ
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = item.Key;
            }
        }

        if (oldEquipment != null)
        {
            //ж��װ��
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();

        RemoveItem(_item);

        UpdateSlotUI();
    }

    /// <summary>
    /// ж��װ��
    /// </summary>
    /// <param name="itemToRemove"></param>
    public void UnequipItem(ItemData_Equipment itemToRemove)
    {
        //key Ҫ��ȡ��ֵ�ļ�
        //value ���˷�������ֵʱ������ҵ��ü�����᷵����ָ���ļ��������ֵ��������᷵�� value ����������Ĭ��ֵ�� �˲���δ����ʼ���������ݡ�
        //out�������������
        //������������֮ǰ�����Բ����ȸ�ֵ��
        //�ڷ����ڲ�������Ҫ�и�������ֵ�����
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
        }
    }

    /// <summary>
    /// ���²�λUI
    /// </summary>
    private void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            //1��KeyValuePair 
            //a��KeyValuePair ��һ���ṹ�壨struct����
            //b��KeyValuePair ֻ����һ��Key��Value�ļ�ֵ��
            // �����ֵ��������ֵ
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlots[i].slotType)
                {
                    equipmentSlots[i].UpdateSlot(item.Value);
                }
            }
        }

        for (int i = 0; i < inventoryItemSlots.Length; i++)
        {
            inventoryItemSlots[i].CleanUpSlot();
        }
        for (int i = 0; i < stashItemSlots.Length; i++)
        {
            stashItemSlots[i].CleanUpSlot();
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlots[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlots[i].UpdateSlot(stash[i]);
        }

        UpdateStatsUI();
    }

    /// <summary>
    /// ����ͳ��ֵUI
    /// </summary>
    public void UpdateStatsUI()
    {
        for (int i = 0; i < statSlots.Length; i++)
        {
            statSlots[i].UpdateStatValueUI();
        }
    }

    /// <summary>
    /// �����Ʒ
    /// </summary>
    /// <param name="_item"></param>
    public void AddItem(ItemData _item)
    {
        //װ����ӽ����
        if (_item.itemType == ItemType.Equipment && CanAddItem())
        {
            AddToInventory(_item);
        }
        //������ӽ�������
        else if (_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
        }


        UpdateSlotUI();
    }

    /// <summary>
    /// ��ӵ�������
    /// </summary>
    /// <param name="_item"></param>
    private void AddToStash(ItemData _item)
    {
        //key Ҫ��ȡ��ֵ�ļ�
        //value ���˷�������ֵʱ������ҵ��ü�����᷵����ָ���ļ��������ֵ��������᷵�� value ����������Ĭ��ֵ�� �˲���δ����ʼ���������ݡ�
        //out�������������
        //������������֮ǰ�����Բ����ȸ�ֵ��
        //�ڷ����ڲ�������Ҫ�и�������ֵ�����
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            //����ڴ��������ҵ�������Ʒ�����Ǿ�Ϊ����ӳߴ�
            value.AddStack();
        }
        else
        {
            //û�о������
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    /// <summary>
    /// ��ӵ����
    /// </summary>
    /// <param name="_item"></param>
    private void AddToInventory(ItemData _item)
    {
        //key Ҫ��ȡ��ֵ�ļ�
        //value ���˷�������ֵʱ������ҵ��ü�����᷵����ָ���ļ��������ֵ��������᷵�� value ����������Ĭ��ֵ�� �˲���δ����ʼ���������ݡ�
        //out�������������
        //������������֮ǰ�����Բ����ȸ�ֵ��
        //�ڷ����ڲ�������Ҫ�и�������ֵ�����
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            //����ڿ�����ҵ�������Ʒ�����Ǿ�Ϊ����ӳߴ�
            value.AddStack();
        }
        else
        {
            //û�о������
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    /// <summary>
    /// ɾ����Ʒ
    /// </summary>
    /// <param name="_item"></param>
    public void RemoveItem(ItemData _item)
    {
        //key Ҫ��ȡ��ֵ�ļ�
        //value ���˷�������ֵʱ������ҵ��ü�����᷵����ָ���ļ��������ֵ��������᷵�� value ����������Ĭ��ֵ�� �˲���δ����ʼ���������ݡ�
        //out�������������
        //������������֮ǰ�����Բ����ȸ�ֵ��
        //�ڷ����ڲ�������Ҫ�и�������ֵ�����
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem inventoryValue))
        {
            //����ڿ�����ҵ�������Ʒ�����Ǿ�Ϊ����ӳߴ�
            if (inventoryValue.stackSize <= 1)
            {
                inventory.Remove(inventoryValue);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                inventoryValue.RemoveStack();
            }
        }

        //key Ҫ��ȡ��ֵ�ļ�
        //value ���˷�������ֵʱ������ҵ��ü�����᷵����ָ���ļ��������ֵ��������᷵�� value ����������Ĭ��ֵ�� �˲���δ����ʼ���������ݡ�
        //out�������������
        //������������֮ǰ�����Բ����ȸ�ֵ��
        //�ڷ����ڲ�������Ҫ�и�������ֵ�����
        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            //����ڿ�����ҵ�������Ʒ�����Ǿ�Ϊ����ӳߴ�
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }

        UpdateSlotUI();
    }

    /// <summary>
    /// �ܷ������Ʒ
    /// </summary>
    /// <returns></returns>
    public bool CanAddItem()
    {
        if (inventory.Count >= inventoryItemSlots.Length)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// �ܷ��ֹ�����
    /// </summary>
    /// <param name="_itemToCraft">װ������</param>
    /// <param name="_requiredMaterials">�����Ʒ</param>
    /// <returns></returns>
    public bool CanCraft(ItemData_Equipment _itemToCraft,List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();

        for (int i = 0; i < _requiredMaterials.Count; i++)
        {
            //key Ҫ��ȡ��ֵ�ļ�
            //value ���˷�������ֵʱ������ҵ��ü�����᷵����ָ���ļ��������ֵ��������᷵�� value ����������Ĭ��ֵ�� �˲���δ����ʼ���������ݡ�
            //out�������������
            //������������֮ǰ�����Բ����ȸ�ֵ��
            //�ڷ����ڲ�������Ҫ�и�������ֵ�����
            if (stashDictionary.TryGetValue(_requiredMaterials[i].itemData , out InventoryItem stashValue))
            {
                Debug.Log("stashValue.stackSize=" + stashValue.stackSize);
                Debug.Log("_requiredMaterials[i].stackSize=" + _requiredMaterials[i].stackSize);
                //���ʵ���еĲ�����������Ҫ�Ĳ���
                if (stashValue.stackSize < _requiredMaterials[i].stackSize)
                {
                    return false;
                }
                else
                {
                    for (int j = 0; j < _requiredMaterials[i].stackSize; j++)
                    {
                        materialsToRemove.Add(stashValue);
                    }
                    
                }
            }
            else
            {
                return false;
            }
        }
        Debug.Log("materialsToRemove=" + materialsToRemove);
        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].itemData);
        }

        AddItem(_itemToCraft);

        return true;
    }

    /// <summary>
    /// ��ȡװ���嵥
    /// </summary>
    /// <returns></returns>
    public List<InventoryItem> GetEquipmentList()
    {
        return equipment;
    }

    /// <summary>
    /// ��ȡ�������嵥
    /// </summary>
    /// <returns></returns>
    public List<InventoryItem> GetStashList()
    {
        return stash;
    }

    /// <summary>
    /// ����װ�����ͻ�ȡװ��
    /// </summary>
    /// <param name="_type">װ������</param>
    /// <returns></returns>
    public ItemData_Equipment GetEquipment(EquipmentType _type)
    {
        ItemData_Equipment equipedItem = null;

        //1��KeyValuePair 
        //a��KeyValuePair ��һ���ṹ�壨struct����
        //b��KeyValuePair ֻ����һ��Key��Value�ļ�ֵ��
        // �����ֵ��������ֵ
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)
            {
                equipedItem = item.Key;
            }
        }

        return equipedItem;
    }

    /// <summary>
    /// ʹ��ƿ��
    /// </summary>
    public void UseFlask()
    {
        ItemData_Equipment currentFlask = GetEquipment(EquipmentType.Flask);

        if (currentFlask == null)
        {
            return;
        }

        bool canUseFlask = Time.time > lastTimeUsedFlask + flaskCooldown;

        if (canUseFlask)
        {
            flaskCooldown = currentFlask.itemCooldown;
            currentFlask.Effect(null);
            lastTimeUsedFlask = Time.time;
        }
        else
        {
            Debug.Log("2");
        }
    }

    /// <summary>
    /// �Ƿ�ʹ�û���
    /// </summary>
    /// <returns></returns>
    public bool CanUseArmor()
    {
        ItemData_Equipment currentArmor = GetEquipment(EquipmentType.Armor);

        if (currentArmor == null)
        {
            return false;
        }

        if (Time.time > lastTimeUsedArmor + armorCooldown)
        {
            armorCooldown = currentArmor.itemCooldown;
            lastTimeUsedArmor = Time.time;
            return true;
        }

        return false;
    }
}
