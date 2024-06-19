using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 在 Assets 下添加菜单
// fileName 生成名为 Freeze enemeis effect的脚本
// menuName 菜单按钮名Data/Item effect/Freeze enemeis effect
// order    按钮显示顺序
[CreateAssetMenu(fileName = "Freeze enemeis effect", menuName = "Data/Item effect/Freeze enemeis")]
/// <summary>
/// 冻结敌人效果
/// </summary>
public class FreezeEnemies_Effect : ItemEffect
{
    //持续时间
    [SerializeField] private float duration;

    /// <summary>
    /// 执行的效果
    /// </summary>
    /// <param name="_transform"></param>
    public override void ExecuteEffect(Transform _transform)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        //血量低于10%才触发
        if (playerStats.currentHealth > playerStats.GetMaxHealthValue() * 0.1f)
        {
            return;
        }

        if (!Inventory.instance.CanUseArmor())
        {
            return;
        }

        //获取位于圆形区域内的所有2D游戏碰撞体的列表  
        //第一参数是 point	圆形的中心。
        //第二参数是 radius	圆形的半径。
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2);
        //遍历碰撞器包，检测是否存在敌人，如果存在，则调用敌人组件中的Damage函数
        foreach (var hit in colliders)
        {
            hit.GetComponent<Enemy>().FreezeTimeFor(duration);    
        }
    }
}
