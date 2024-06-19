using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 骨架死亡
/// </summary>
public class SkeletonDeadState : EnemyState
{
    private Enemy_Skeleton enemy;
    public SkeletonDeadState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_Skeleton enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.animator.SetBool(enemy.lastAnimBoolname, true);
        enemy.animator.speed = 0;
        //胶囊形状的原始对撞机脚本关闭
        enemy.capsuleCollider2D.enabled = false;

        stateTimer = 0.15f;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            rigidbody2D.velocity = new Vector2(0, 10);
        }
    }
}
