using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 技能
/// </summary>
public class Skill : MonoBehaviour
{
    //冷却时间
    [SerializeField] protected float cooldown;
    //冷却定时器
    protected float cooldownTimer;

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    /// <summary>
    /// 是否可以用技能
    /// </summary>
    /// <returns></returns>
    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        return false;
    }

    /// <summary>
    /// 做一些特定技能的事情
    /// </summary>
    public virtual void UseSkill()
    {

    }

    /// <summary>
    /// 找到最近的敌人
    /// </summary>
    /// <returns></returns>
    protected virtual Transform FindClosestEnemy(Transform _checkTransform)
    {
        //获取位于圆形区域内的所有2D游戏碰撞体的列表  
        //第一参数是 point	圆形的中心。
        //第二参数是 radius	圆形的半径。
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25);
        //最近距离
        float closestDistance = Mathf.Infinity;
        //最近的怪物
        Transform closestEnemy = null;

        //遍历碰撞器包，检测是否存在敌人，如果存在，则调用敌人组件中的Damage函数
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                //返回A到B之间的距离
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }
}
