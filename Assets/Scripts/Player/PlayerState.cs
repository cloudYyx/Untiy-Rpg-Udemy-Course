using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家状态
/// </summary>
public class PlayerState
{
    //状态机
    protected PlayerStateMachine stateMachine;
    //玩家
    protected Player player;
    //刚体
    protected Rigidbody2D rigidbody2D;
    //水平输入
    protected float xInput;
    //垂直输入
    protected float yInput;
    //动画名字
    private string animBoolName;
    //定时器
    protected float stateTimer;
    //触发器
    protected bool triggerCalled;

    public PlayerState(PlayerStateMachine stateMachine, Player player, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.player = player;
        this.animBoolName = animBoolName;
    }

    /// <summary>
    /// 进入
    /// </summary>
    public virtual void Enter()
    {
        player.animator.SetBool(animBoolName, true);
        rigidbody2D = player.GetComponent<Rigidbody2D>();
        triggerCalled = false;
    }
    /// <summary>
    /// 更新
    /// </summary>
    public virtual void Update()
    {
        //time -=  两帧之间的时间
        stateTimer -= Time.deltaTime;

        //获取水平轴输入
        xInput = Input.GetAxisRaw("Horizontal");
        //获取垂直轴输入
        yInput = Input.GetAxisRaw("Vertical");

        player.animator.SetFloat("yVelocity", rigidbody2D.velocity.y);
    }
    /// <summary>
    /// 退出
    /// </summary>
    public virtual void Exit() 
    {
        player.animator.SetBool(animBoolName, false);
    }
    /// <summary>
    /// 动画触发器
    /// </summary>
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
