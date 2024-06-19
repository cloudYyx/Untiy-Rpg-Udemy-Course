using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SkeletonBattleState : EnemyState
{
    //对象的位置、旋转和比例。
    private Transform player;
    private Enemy_Skeleton enemy;
    //移动方向，正数为右，负数为左
    private int moveDir;

    public SkeletonBattleState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_Skeleton enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                    return;
                }
            }
        }
        else
        {
            //定时器小于0，怪物和玩家距离超过界限
            if (stateTimer < 0 || Vector2.Distance(player.transform.position,enemy.transform.position) > 10)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }

        if (player.position.x > enemy.transform.position.x)
        {
            moveDir = 1;
        }else if (player.position.x < enemy.transform.position.x)
        {
            moveDir = -1;
        }

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rigidbody2D.velocity.y);
    }

    /// <summary>
    /// 是否能攻击
    /// </summary>
    /// <returns></returns>
    private bool CanAttack()
    {
        //应用程序已运行的时间（以秒为单位） 大于等于 最后一次攻击时间 + 攻击cd
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}


