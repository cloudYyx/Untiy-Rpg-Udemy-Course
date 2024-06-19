using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��Ʒ���󴥷���
/// </summary>
public class ItemObject_Trigger : MonoBehaviour
{
    private ItemObject myItemObject => GetComponentInParent<ItemObject>();


    /// <summary>
    /// ����һ��������븽�ӵ��ö���Ĵ�����ײ��ʱ���ͣ����� 2D ����
    /// </summary>
    /// <param name="collision">����ײ���漰������ Collider2D</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (collision.GetComponent<CharacterStats>().isDead)
            {
                return;
            }

            myItemObject.PickupItem();
        }
    }
}
