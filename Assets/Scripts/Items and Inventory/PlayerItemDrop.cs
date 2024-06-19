using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
/// <summary>
/// 玩家掉落物品
/// </summary>
public class PlayerItemDrop : ItemDrop
{
    //玩家掉落
    [Header("Player's drop")]
    //丢失物品的机会
    [SerializeField] protected float chanceToLooseItems;
    //丢失材料的机会
    [SerializeField] protected float chanceToLooseMaterials;

    /// <summary>
    /// 生成掉落
    /// </summary>
    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;

        //储藏室清单
        List<InventoryItem> currentStash = inventory.GetStashList();
        //装备清单
        List<InventoryItem> currentEquipment = inventory.GetEquipmentList();
        //要丢失的装备
        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
        //要丢失的材料
        List<InventoryItem> materulsToLoose = new List<InventoryItem>();

        //装备丢失 start
        for (int i = 0; i < currentEquipment.Count ; i++)
        {
            if (Random.Range(0, 100) <= chanceToLooseItems)
            {
                //掉落装备
                DropItem(currentEquipment[i].itemData);
                itemsToUnequip.Add(currentEquipment[i]);
            }
        }

        for (int i = 0; i < itemsToUnequip.Count; i++)
        {
            //丢失装备
            inventory.UnequipItem(itemsToUnequip[i].itemData as ItemData_Equipment);
        }
        //装备丢失 end

        //材料丢失 start
        for (int i = 0; i < currentStash.Count; i++)
        {
            if (Random.Range(0, 100) <= chanceToLooseMaterials)
            {
                //掉落装备
                DropItem(currentStash[i].itemData);
                materulsToLoose.Add(currentStash[i]);
            }
        }

        for (int i = 0; i < materulsToLoose.Count; i++)
        {
            //丢失材料
            inventory.RemoveItem(materulsToLoose[i].itemData);
        }
        //材料丢失 end
    }
}
