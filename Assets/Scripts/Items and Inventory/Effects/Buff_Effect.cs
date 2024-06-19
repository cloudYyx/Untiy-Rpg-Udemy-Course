using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 在 Assets 下添加菜单
// fileName 生成名为 Buff effect的脚本
// menuName 菜单按钮名Data/Item effect/Buff effect
// order    按钮显示顺序
[CreateAssetMenu(fileName = "Buff effect", menuName = "Data/Item effect/Buff effect")]
/// <summary>
/// 增益效果
/// </summary>
public class Buff_Effect : ItemEffect
{
    //玩家属性统计
    private PlayerStats stats;
    //增益类型
    [SerializeField] private StatType buffType;
    //增益数量
    [SerializeField] private int buffAmount;
    //增益持续时间
    [SerializeField] private float buffDuration;

    /// <summary>
    /// 执行的效果
    /// </summary>
    /// <param name="_enemyPosition"></param>
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        stats.IncreaseStatBy(buffAmount, buffDuration, stats.GetStat(buffType));
    }
}
