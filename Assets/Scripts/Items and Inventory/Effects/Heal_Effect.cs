using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 在 Assets 下添加菜单
// fileName 生成名为 Heal effect的脚本
// menuName 菜单按钮名Data/Item effect/Heal effect
// order    按钮显示顺序
[CreateAssetMenu(fileName = "Heal effect", menuName = "Data/Item effect/Heal effect")]
/// <summary>
/// 治愈效果
/// </summary>
public class Heal_Effect : ItemEffect
{
    [Range(0f, 1f)]
    //治愈百分比
    [SerializeField] private float healPercent;

    /// <summary>
    /// 执行的效果
    /// </summary>
    /// <param name="_enemyPosition"></param>
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        //治疗量
        int healAmount = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPercent);

        playerStats.IncreaseHealthBy(healAmount);        
    }
}
