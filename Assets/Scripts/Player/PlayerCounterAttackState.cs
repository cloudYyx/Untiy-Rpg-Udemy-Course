using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家反击状态
/// </summary>
public class PlayerCounterAttackState : PlayerState
{
    //是否创建克隆
    private bool canCreateClone;

    public PlayerCounterAttackState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        canCreateClone = true;
        stateTimer = player.counterAttackDuration;
        player.animator.SetBool("SuccessfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        //获取位于圆形区域内的所有2D游戏碰撞体的列表  
        //第一参数是 point	圆形的中心。
        //第二参数是 radius	圆形的半径。
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        //遍历碰撞器包，检测是否存在敌人，如果存在，则调用敌人组件中的Damage函数
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10; //任何大于动画时间的值
                    player.animator.SetBool("SuccessfulCounterAttack", true);

                    //用来恢复队友生命值
                    player.skillManager.parry.UseSkill();

                    if (canCreateClone)
                    {
                        canCreateClone = false;
                        player.skillManager.parry.MakeMirageOnParry(hit.transform);
                    }
                }
            }
        }

        if (stateTimer < 0 || triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
