using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��ҷ���״̬
/// </summary>
public class PlayerCounterAttackState : PlayerState
{
    //�Ƿ񴴽���¡
    private bool canCreateClone;

    public PlayerCounterAttackState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        canCreateClone = true;
        stateTimer = player.counterAttackDuration;
        player.animator.SetBool("SuccessfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        //��ȡλ��Բ�������ڵ�����2D��Ϸ��ײ����б�  
        //��һ������ point	Բ�ε����ġ�
        //�ڶ������� radius	Բ�εİ뾶��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        //������ײ����������Ƿ���ڵ��ˣ�������ڣ�����õ�������е�Damage����
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10; //�κδ��ڶ���ʱ���ֵ
                    player.animator.SetBool("SuccessfulCounterAttack", true);

                    //�����ָ���������ֵ
                    player.skillManager.parry.UseSkill();

                    if (canCreateClone)
                    {
                        canCreateClone = false;
                        player.skillManager.parry.MakeMirageOnParry(hit.transform);
                    }
                }
            }
        }

        if (stateTimer < 0 || triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
