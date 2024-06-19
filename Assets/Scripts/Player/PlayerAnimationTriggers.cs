using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家动画触发器
/// 把脚本添加进动画预制体
/// </summary>
public class PlayerAnimationTriggers : MonoBehaviour
{
    /// <summary>
    /// 不想创建一个启动函数，但是想访问组件。可以这样做，获取父类组件
    /// </summary>
    private Player player => GetComponentInParent<Player>();

    /// <summary>
    /// 动画触发器方法
    /// 在动画窗口-样本右边-添加事件里调用此方法可以让触发器变为ture
    /// </summary>
    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    /// <summary>
    /// 动画触发器方法
    /// 在动画窗口-样本右边-添加事件里调用此方法可以让触发器变为ture
    /// </summary>
    private void AttackTrigger()
    {
        //获取位于圆形区域内的所有2D游戏碰撞体的列表  
        //第一参数是 point	圆形的中心。
        //第二参数是 radius	圆形的半径。
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        //遍历碰撞器包，检测是否存在敌人，如果存在，则调用敌人组件中的Damage函数
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();

                if (_target != null)
                {
                    player.characterState.DoDamage(_target);
                }

                WeaponEffect(_target);
            }
        }
    }

    /// <summary>
    /// 武器的效果
    /// </summary>
    private void WeaponEffect(EnemyStats _target)
    {
        ItemData_Equipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);

        if (weaponData != null)
        {
            weaponData.Effect(_target.transform);
        }
    }

    /// <summary>
    /// 扔剑触发器
    /// </summary>
    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
