using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
/// <summary>
/// ��ҵ�����Ʒ
/// </summary>
public class PlayerItemDrop : ItemDrop
{
    //��ҵ���
    [Header("Player's drop")]
    //��ʧ��Ʒ�Ļ���
    [SerializeField] protected float chanceToLooseItems;
    //��ʧ���ϵĻ���
    [SerializeField] protected float chanceToLooseMaterials;

    /// <summary>
    /// ���ɵ���
    /// </summary>
    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;

        //�������嵥
        List<InventoryItem> currentStash = inventory.GetStashList();
        //װ���嵥
        List<InventoryItem> currentEquipment = inventory.GetEquipmentList();
        //Ҫ��ʧ��װ��
        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
        //Ҫ��ʧ�Ĳ���
        List<InventoryItem> materulsToLoose = new List<InventoryItem>();

        //װ����ʧ start
        for (int i = 0; i < currentEquipment.Count ; i++)
        {
            if (Random.Range(0, 100) <= chanceToLooseItems)
            {
                //����װ��
                DropItem(currentEquipment[i].itemData);
                itemsToUnequip.Add(currentEquipment[i]);
            }
        }

        for (int i = 0; i < itemsToUnequip.Count; i++)
        {
            //��ʧװ��
            inventory.UnequipItem(itemsToUnequip[i].itemData as ItemData_Equipment);
        }
        //װ����ʧ end

        //���϶�ʧ start
        for (int i = 0; i < currentStash.Count; i++)
        {
            if (Random.Range(0, 100) <= chanceToLooseMaterials)
            {
                //����װ��
                DropItem(currentStash[i].itemData);
                materulsToLoose.Add(currentStash[i]);
            }
        }

        for (int i = 0; i < materulsToLoose.Count; i++)
        {
            //��ʧ����
            inventory.RemoveItem(materulsToLoose[i].itemData);
        }
        //���϶�ʧ end
    }
}
