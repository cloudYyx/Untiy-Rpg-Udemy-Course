using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �����Ҫ����״̬
/// </summary>
public class PlayerPrimaryAttackState : PlayerState
{
    //���н׶�
    public int comboCounter {  get; private set; }
    //���һ�ι���ʱ��
    private float lastTimeAttacked;
    //�������ڣ�������ʼ�������������
    private float comboWindow = 2;
    //��������
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

        
        //�����ˮƽ������룬���������Ϊ���뷽��
        if (xInput != 0) 
        {
            attackDir = xInput;
        }
        else
        {
            attackDir = player.facingDir;
        }

        //���������ƶ�����
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
