using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �׵������
/// </summary>
public class ThunderStrike_Controller : MonoBehaviour
{
    /// <summary>
    //�����һ����ײ�����봥��������������
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
