using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Íæ¼ÒÒÆ¶¯×´Ì¬
/// </summary>
public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.moveSpeed, rigidbody2D.velocity.y);

        if (xInput == 0 || player.IsWallDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
