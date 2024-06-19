using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家用剑瞄准状态
/// </summary>
public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skillManager.sword.DotsActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        //启动一个名为“BusyFor”的协程,持续时间0.2f
        player.StartCoroutine("BusyFor", 0.2f);
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            stateMachine.ChangeState(player.idleState);
        }
        //        //此方法的作用是将参考点Input.mousePosition从屏幕坐标系转换到世界坐标系
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (player.transform.position.x > mousePosition.x && player.facingDir == 1)
        {
            player.Flip();
        }else if (player.transform.position.x < mousePosition.x && player.facingDir == -1)
        {
            player.Flip();
        }
    }
}
