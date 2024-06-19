using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ѣ��״̬
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
        //�� time ������ methodName ������Ȼ��ÿ repeatRate �����һ�Ρ�
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
