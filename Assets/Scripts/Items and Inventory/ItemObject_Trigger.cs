using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 物品对象触发器
/// </summary>
public class ItemObject_Trigger : MonoBehaviour
{
    private ItemObject myItemObject => GetComponentInParent<ItemObject>();


    /// <summary>
    /// 当另一个对象进入附加到该对象的触发碰撞体时发送（仅限 2D 物理）
    /// </summary>
    /// <param name="collision">该碰撞中涉及的其他 Collider2D</param>
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
