using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Íæ¼ÒÔÚÇ½ÉÏÌøÔ¾
/// </summary>
public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = 1f;
        player.SetVelocity(5 * -player.facingDir, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.airState);
        }

        if (player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
