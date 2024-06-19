using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
/// <summary>
/// 库存
/// </summary>
public class Inventory : MonoBehaviour
{
    //实例
    public static Inventory instance;

    //起始物品
    public List<ItemData> startingItems;

    //装备
    public List<InventoryItem> equipment;
    //字典中key只能对应一个值不能对应多个值，线性结构。 相当于JAVA的MAP
    //装备的字典
    //实例化：Dictionary<键key, 值value> 名字dic = new Dictionary<键key, 值value>（）;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    //库存
    public List<InventoryItem> inventory;
    public Dictionary<ItemData,InventoryItem> inventoryDictionary;

    //储藏室
    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    //库存UI
    [Header("Inventory UI")]
    //库存槽（父节点，容纳所有的物品槽，背包）
    [SerializeField] private Transform inventorySlotParent;
    //储藏室槽（父节点,仓库）
    [SerializeField] private Transform stashSlotParent;
    //装备槽(父节点)
    [SerializeField] private Transform equpmenntSlotParent;
    //统计槽(父节点)
    [SerializeField] private Transform statSlotParent;

    //库存物品槽位
    private UI_ItemSlot[] inventoryItemSlots;
    //储藏室物品槽位
    private UI_ItemSlot[] stashItemSlots;
    //装备槽位
    private UI_EquipmentSlot[] equipmentSlots;
    //统计槽
    private UI_StatSlot[] statSlots;

    //物品冷却
    [Header("Item cooldown")]
    //上次使用瓶子时间
    private float lastTimeUsedFlask;
    //上次使用武器时间
    private float lastTimeUsedArmor;

    //瓶子冷却
    private float flaskCooldown;
    //护甲冷却
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

        //GetComponents获取所有组件的引用
        inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlots = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlots = equpmenntSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        statSlots = statSlotParent.GetComponentsInChildren<UI_StatSlot>();

        AddStartingItems();
    }

    /// <summary>
    /// 添加起始物品
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
    /// 装备物品
    /// </summary>
    /// <param name="_item"></param>
    public void EquipItem(ItemData _item)
    {
        //as:与强制类型转换是一样的,但是永远不会抛出异常,即如果转换不成功,会返回null
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null;

        //1、KeyValuePair 
        //a、KeyValuePair 是一个结构体（struct）；
        //b、KeyValuePair 只包含一个Key、Value的键值对
        // 遍历字典输出键与值
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = item.Key;
            }
        }

        if (oldEquipment != null)
        {
            //卸载装备
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
    /// 卸载装备
    /// </summary>
    /// <param name="itemToRemove"></param>
    public void UnequipItem(ItemData_Equipment itemToRemove)
    {
        //key 要获取的值的键
        //value 当此方法返回值时，如果找到该键，便会返回与指定的键相关联的值；否则，则会返回 value 参数的类型默认值。 此参数未经初始化即被传递。
        //out：：输出参数。
        //传方法到参数之前，可以不用先赋值。
        //在方法内部，必须要有给参数赋值的语句
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
        }
    }

    /// <summary>
    /// 更新槽位UI
    /// </summary>
    private void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            //1、KeyValuePair 
            //a、KeyValuePair 是一个结构体（struct）；
            //b、KeyValuePair 只包含一个Key、Value的键值对
            // 遍历字典输出键与值
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
    /// 更新统计值UI
    /// </summary>
    public void UpdateStatsUI()
    {
        for (int i = 0; i < statSlots.Length; i++)
        {
            statSlots[i].UpdateStatValueUI();
        }
    }

    /// <summary>
    /// 添加物品
    /// </summary>
    /// <param name="_item"></param>
    public void AddItem(ItemData _item)
    {
        //装备添加进库存
        if (_item.itemType == ItemType.Equipment && CanAddItem())
        {
            AddToInventory(_item);
        }
        //材料添加进储藏室
        else if (_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
        }


        UpdateSlotUI();
    }

    /// <summary>
    /// 添加到储藏室
    /// </summary>
    /// <param name="_item"></param>
    private void AddToStash(ItemData _item)
    {
        //key 要获取的值的键
        //value 当此方法返回值时，如果找到该键，便会返回与指定的键相关联的值；否则，则会返回 value 参数的类型默认值。 此参数未经初始化即被传递。
        //out：：输出参数。
        //传方法到参数之前，可以不用先赋值。
        //在方法内部，必须要有给参数赋值的语句
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            //如果在储藏室里找到这种物品，我们就为它添加尺寸
            value.AddStack();
        }
        else
        {
            //没有就新添加
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    /// <summary>
    /// 添加到库存
    /// </summary>
    /// <param name="_item"></param>
    private void AddToInventory(ItemData _item)
    {
        //key 要获取的值的键
        //value 当此方法返回值时，如果找到该键，便会返回与指定的键相关联的值；否则，则会返回 value 参数的类型默认值。 此参数未经初始化即被传递。
        //out：：输出参数。
        //传方法到参数之前，可以不用先赋值。
        //在方法内部，必须要有给参数赋值的语句
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            //如果在库存里找到这种物品，我们就为它添加尺寸
            value.AddStack();
        }
        else
        {
            //没有就新添加
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    /// <summary>
    /// 删除物品
    /// </summary>
    /// <param name="_item"></param>
    public void RemoveItem(ItemData _item)
    {
        //key 要获取的值的键
        //value 当此方法返回值时，如果找到该键，便会返回与指定的键相关联的值；否则，则会返回 value 参数的类型默认值。 此参数未经初始化即被传递。
        //out：：输出参数。
        //传方法到参数之前，可以不用先赋值。
        //在方法内部，必须要有给参数赋值的语句
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem inventoryValue))
        {
            //如果在库存里找到这种物品，我们就为它添加尺寸
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

        //key 要获取的值的键
        //value 当此方法返回值时，如果找到该键，便会返回与指定的键相关联的值；否则，则会返回 value 参数的类型默认值。 此参数未经初始化即被传递。
        //out：：输出参数。
        //传方法到参数之前，可以不用先赋值。
        //在方法内部，必须要有给参数赋值的语句
        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            //如果在库存里找到这种物品，我们就为它添加尺寸
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
    /// 能否添加物品
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
    /// 能否手工制作
    /// </summary>
    /// <param name="_itemToCraft">装备数据</param>
    /// <param name="_requiredMaterials">库存物品</param>
    /// <returns></returns>
    public bool CanCraft(ItemData_Equipment _itemToCraft,List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();

        for (int i = 0; i < _requiredMaterials.Count; i++)
        {
            //key 要获取的值的键
            //value 当此方法返回值时，如果找到该键，便会返回与指定的键相关联的值；否则，则会返回 value 参数的类型默认值。 此参数未经初始化即被传递。
            //out：：输出参数。
            //传方法到参数之前，可以不用先赋值。
            //在方法内部，必须要有给参数赋值的语句
            if (stashDictionary.TryGetValue(_requiredMaterials[i].itemData , out InventoryItem stashValue))
            {
                Debug.Log("stashValue.stackSize=" + stashValue.stackSize);
                Debug.Log("_requiredMaterials[i].stackSize=" + _requiredMaterials[i].stackSize);
                //如果实际有的材料少于所需要的材料
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
    /// 获取装备清单
    /// </summary>
    /// <returns></returns>
    public List<InventoryItem> GetEquipmentList()
    {
        return equipment;
    }

    /// <summary>
    /// 获取储藏室清单
    /// </summary>
    /// <returns></returns>
    public List<InventoryItem> GetStashList()
    {
        return stash;
    }

    /// <summary>
    /// 根据装备类型获取装备
    /// </summary>
    /// <param name="_type">装备类型</param>
    /// <returns></returns>
    public ItemData_Equipment GetEquipment(EquipmentType _type)
    {
        ItemData_Equipment equipedItem = null;

        //1、KeyValuePair 
        //a、KeyValuePair 是一个结构体（struct）；
        //b、KeyValuePair 只包含一个Key、Value的键值对
        // 遍历字典输出键与值
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
    /// 使用瓶子
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
    /// 是否使用护甲
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
