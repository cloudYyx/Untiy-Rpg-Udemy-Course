using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家接地状态
/// </summary>
public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
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

        if (Input.GetKeyDown(KeyCode.R) && player.skillManager.blackhole.blackholeUnlocked)
        {
            stateMachine.ChangeState(player.blackHole);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword() && player.skillManager.sword.swordUnlocked)
        {
            stateMachine.ChangeState(player.aimSword);
        }

        if (Input.GetKeyDown(KeyCode.Q) && player.skillManager.parry.parryUnlocked)
        {
            stateMachine.ChangeState(player.counterAttack);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.playerPrimaryAttack);
        }

        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.jumpState);
        }
    }

    /// <summary>
    /// 检查是否有剑
    /// </summary>
    /// <returns></returns>
    private bool HasNoSword()
    {
        if (!player.sword)
        {
            return true;
        }

        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }
}
