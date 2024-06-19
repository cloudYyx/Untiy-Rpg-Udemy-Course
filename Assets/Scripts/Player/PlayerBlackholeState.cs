using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��Һڶ�״̬
/// </summary>
public class PlayerBlackholeState : PlayerState
{
    //����ʱ��
    private float flyTime = 0.4f;
    //�����Ƿ�ʹ��
    private bool skillUsed;
    //����
    private float defaultGravity;

    public PlayerBlackholeState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        defaultGravity = player.rb.gravityScale;

        skillUsed = false;
        stateTimer = flyTime;
        //����
        rigidbody2D.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();
        player.rb.gravityScale = defaultGravity;
        player.entityFX.MakeTransprent(false);
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer > 0)
        {
            rigidbody2D.velocity = new Vector2(0, 15);
        }

        if (stateTimer < 0)
        {
            rigidbody2D.velocity = new Vector2(0, -0.1f);

            if (!skillUsed)
            {
                if (player.skillManager.blackhole.CanUseSkill())
                {
                    skillUsed = true;
                }
            }
        }

        if (player.skillManager.blackhole.SkillCompleted())
        {
            stateMachine.ChangeState(player.airState);
        }
    }
}
