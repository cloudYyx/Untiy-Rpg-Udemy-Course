using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家冲刺状态
/// </summary>
public class PlayerDashState : PlayerState
{
    public PlayerDashState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skillManager.dash.CloneOnDash();

        stateTimer = player.dashDuration;
    }

    public override void Exit()
    {
        base.Exit();

        player.skillManager.dash.CloneOnArrival();

        player.SetVelocity(0, rigidbody2D.velocity.y);

    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected() && player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        //冲刺力 x 面朝方向（1为右，-1为左）
        player.SetVelocity(player.dashSpeed * player.dashDir, 0);

        if (stateTimer < 0) 
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
