using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 雷电控制器
/// </summary>
public class ThunderStrike_Controller : MonoBehaviour
{
    /// <summary>
    //如果另一个碰撞器进入触发器，则调用这个
    /// </summary>
    /// <param name="collision"></param>
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
            EnemyStats enemyTarget = collision.GetComponent<EnemyStats>();
            playerStats.DoMagicalDamage(enemyTarget);
        }
    }
}
