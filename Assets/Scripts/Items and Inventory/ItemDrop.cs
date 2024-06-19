using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// ������Ʒ
/// </summary>
public class ItemDrop : MonoBehaviour
{
    //���ܵ������Ʒ
    [SerializeField] private int possibleItemDrop;
    //���ܵĵ���
    [SerializeField] private ItemData[] possibleDrop;
    //�����б�
    private List<ItemData> dropList = new List<ItemData>();

    //����Ԥ����
    [SerializeField] private GameObject dropPrefab;

    /// <summary>
    /// ���ɵ���
    /// </summary>
    public virtual void GenerateDrop()
    {
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            //���������������������������ӽ������б�
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
            
            //���ѡ��һ������
            ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];

            dropList.Remove(randomItem);
            DropItem(randomItem);
        }
    }

    /// <summary>
    /// ������Ʒ
    /// </summary>
    protected void DropItem(ItemData _itemData)
    {
        //Object ʵ�����Ŀ�¡����
        //swordPrefab��Ҫ���Ƶ����ж���
        //position	�¶����λ�á�
        //rotation �¶���ķ���
        GameObject newDrop = Instantiate(dropPrefab,transform.position,Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
