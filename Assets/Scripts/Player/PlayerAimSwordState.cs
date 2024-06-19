using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����ý���׼״̬
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
        //����һ����Ϊ��BusyFor����Э��,����ʱ��0.2f
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
        //        //�˷����������ǽ��ο���Input.mousePosition����Ļ����ϵת������������ϵ
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
