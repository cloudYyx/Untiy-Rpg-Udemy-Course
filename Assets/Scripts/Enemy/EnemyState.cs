using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敌人状态
/// </summary>
public class EnemyState
{
    //状态机
    protected EnemyStateMachine stateMachine;
    //敌人
    protected Enemy enemyBase;
    //刚体
    protected Rigidbody2D rigidbody2D;
    //动画名字
    private string animBoolName;
    //触发器
    protected bool triggerCalled;
    //定时器
    protected float stateTimer;

    public EnemyState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.enemyBase = enemyBase;
        this.animBoolName = animBoolName;
    }

    /// <summary>
    /// 更新
    /// </summary>
    public virtual void Update()
    {
        //time -=  两帧之间的时间
        stateTimer -= Time.deltaTime;
    }

    /// <summary>
    /// 进入
    /// </summary>
    public virtual void Enter()
    {
        triggerCalled = false;
        rigidbody2D = enemyBase.rb;
        enemyBase.animator.SetBool(animBoolName, true);
    }

    /// <summary>
    /// 退出
    /// </summary>
    public virtual void Exit()
    {
        enemyBase.animator.SetBool(animBoolName, false);
        enemyBase.AssignLastAnimName(animBoolName);
    }

    /// <summary>
    /// 动画触发器
    /// </summary>
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
