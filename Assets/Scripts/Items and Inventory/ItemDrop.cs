using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// 掉落物品
/// </summary>
public class ItemDrop : MonoBehaviour
{
    //可能掉落的物品
    [SerializeField] private int possibleItemDrop;
    //可能的掉落
    [SerializeField] private ItemData[] possibleDrop;
    //掉落列表
    private List<ItemData> dropList = new List<ItemData>();

    //掉落预制体
    [SerializeField] private GameObject dropPrefab;

    /// <summary>
    /// 生成掉落
    /// </summary>
    public virtual void GenerateDrop()
    {
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            //掉落机会大于随机出来的数字则添加进掉落列表
            if (Random.Range(0,100) <= possibleDrop[i].dropChance)
            {
                dropList.Add(possibleDrop[i]);
            }
        }

        for (int i = 0; i < possibleItemDrop; i++)
        {
            if (!dropList.Any())
            {
                continue;
            }
            
            //随机选择一个掉落
            ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];

            dropList.Remove(randomItem);
            DropItem(randomItem);
        }
    }

    /// <summary>
    /// 掉落物品
    /// </summary>
    protected void DropItem(ItemData _itemData)
    {
        //Object 实例化的克隆对象。
        //swordPrefab是要复制的现有对象。
        //position	新对象的位置。
        //rotation 新对象的方向。
        GameObject newDrop = Instantiate(dropPrefab,transform.position,Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
