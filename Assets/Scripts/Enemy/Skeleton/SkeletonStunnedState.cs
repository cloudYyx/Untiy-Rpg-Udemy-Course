using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 眩晕状态
/// </summary>
public class SkeletonStunnedState : EnemyState
{
    private Enemy_Skeleton enemy;

    public SkeletonStunnedState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_Skeleton enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        //public void InvokeRepeating (string methodName, float time, float repeatRate);
        //在 time 秒后调用 methodName 方法，然后每 repeatRate 秒调用一次。
        enemy.entityFX.InvokeRepeating("RedColorBlink", 0, 0.1f);

        stateTimer = enemy.stunDuration;

        rigidbody2D.velocity = new Vector2(enemy.stunDirection.x * -enemy.facingDir, enemy.stunDirection.y);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.entityFX.Invoke("CancelColorChange", 0);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
