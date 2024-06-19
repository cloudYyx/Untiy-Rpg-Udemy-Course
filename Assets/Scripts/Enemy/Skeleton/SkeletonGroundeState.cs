using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundeState : EnemyState
{
    protected Enemy_Skeleton enemy;

    protected Transform transform;

    public SkeletonGroundeState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_Skeleton enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        transform = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position , transform.position) < 2)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
