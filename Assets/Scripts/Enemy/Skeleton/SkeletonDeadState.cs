using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �Ǽ�����
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
        //������״��ԭʼ��ײ���ű��ر�
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
