using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家主要攻击状态
/// </summary>
public class PlayerPrimaryAttackState : PlayerState
{
    //连招阶段
    public int comboCounter {  get; private set; }
    //最后一次攻击时间
    private float lastTimeAttacked;
    //连击窗口，连击开始经过多久来重置
    private float comboWindow = 2;
    //攻击方向
    private float attackDir;

    public PlayerPrimaryAttackState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
        {
            comboCounter = 0;
        }

        player.animator.SetInteger("ComboCounter", comboCounter);

        
        //如果有水平轴的输入，攻击朝向变为输入方向
        if (xInput != 0) 
        {
            attackDir = xInput;
        }
        else
        {
            attackDir = player.facingDir;
        }

        //攻击动作移动距离
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir,
            player.attackMovement[comboCounter].y);

        stateTimer = 0.1f;
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", 0.15f);

        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            player.SetZeroVelocity();
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
