using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SkeletonBattleState : EnemyState
{
    //�����λ�á���ת�ͱ�����
    private Transform player;
    private Enemy_Skeleton enemy;
    //�ƶ���������Ϊ�ң�����Ϊ��
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
            //��ʱ��С��0���������Ҿ��볬������
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
    /// �Ƿ��ܹ���
    /// </summary>
    /// <returns></returns>
    private bool CanAttack()
    {
        //Ӧ�ó��������е�ʱ�䣨����Ϊ��λ�� ���ڵ��� ���һ�ι���ʱ�� + ����cd
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}


