using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家抓剑状态
/// </summary>
public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;

    public PlayerCatchSwordState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sword = player.sword.transform;

        if (player.transform.position.x > sword.position.x && player.facingDir == 1)
        {
            player.Flip();
        }
        else if (player.transform.position.x < sword.position.x && player.facingDir == -1)
        {
            player.Flip();
        }

        rigidbody2D.velocity = new Vector2 (player.swordReturnImpact * -player.facingDir, rigidbody2D.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();

        //启动一个名为“BusyFor”的协程,持续时间0.2f
        player.StartCoroutine("BusyFor", 0.1f);
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled) 
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
