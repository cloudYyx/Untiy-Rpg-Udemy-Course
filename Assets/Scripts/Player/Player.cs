using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
/// <summary>
/// 玩家
/// </summary>
public class Player : Entity
{
    [Header("Attack details")]
    //攻击动作
    public Vector2[] attackMovement;
    //弹反持续时间
    public float counterAttackDuration = 0.2f;

    //玩家是否在忙
    public bool isBusy {  get; private set; }
    [Header("Move info")]
    //移动速度
    public float moveSpeed = 12f;
    //跳跃力
    public float jumpForce;
    //剑返回影响
    public float swordReturnImpact;
    //默认移动速度
    private float defaultMoveSpeed;
    //默认跳跃力
    private float defaultJumpForce;

    [Header("Dash info")]
    //冲刺力
    public float dashSpeed;
    //冲刺持续时间
    public float dashDuration;
    //冲刺方向
    public float dashDir { get; private set; }
    //默认冲刺速度
    private float defaultDashSpeed;


    //技能管理器
    public SkillManager skillManager { get; private set; }
    //剑
    public GameObject sword { get; private set; }

    #region States
    //获取是公有，修改是私有 == 只读
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState playerWallJumpState { get; private set; }
    public PlayerPrimaryAttackState playerPrimaryAttack { get; private set; }
    public PlayerCounterAttackState counterAttack { get; private set; }
    public PlayerAimSwordState aimSword { get; private set; }
    public PlayerCatchSwordState catchSword { get; private set; }
    public PlayerBlackholeState blackHole { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    #endregion


    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(stateMachine, this, "Idle");
        moveState = new PlayerMoveState(stateMachine, this, "Move");
        jumpState = new PlayerJumpState(stateMachine, this, "Jump");
        airState = new PlayerAirState(stateMachine, this, "Jump");
        dashState = new PlayerDashState(stateMachine, this, "Dash");
        wallSlideState = new PlayerWallSlideState(stateMachine, this, "WallSlide");
        playerWallJumpState = new PlayerWallJumpState(stateMachine, this, "Jump");
        playerPrimaryAttack = new PlayerPrimaryAttackState(stateMachine, this, "Attack");
        counterAttack = new PlayerCounterAttackState(stateMachine, this, "CounterAttack");
        aimSword = new PlayerAimSwordState(stateMachine, this, "AimSword");
        catchSword = new PlayerCatchSwordState(stateMachine, this, "CatchSword");
        blackHole = new PlayerBlackholeState(stateMachine, this, "Jump");
        deadState = new PlayerDeadState(stateMachine, this, "Die");
    }

    protected override void Start()
    {
        base.Start();

        skillManager = SkillManager.instance;

        stateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckForDashInput();

        if (Input.GetKeyDown(KeyCode.F) && skillManager.crystal.crystalUnlocked)
        {
            skillManager.crystal.CanUseSkill();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.instance.UseFlask();
        }
    }

    /// <summary>
    /// 缓慢实体，寒冰元素会使角色缓慢
    /// </summary>
    /// <param name="_slowPercentage">缓慢百分比</param>
    /// <param name="_slowDuration">缓慢持续时间</param>
    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);

        animator.speed = animator.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    /// <summary>
    /// 返回默认速度
    /// </summary>
    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    /// <summary>
    /// 分配新的剑
    /// </summary>
    /// <param name="_newSword"></param>
    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    /// <summary>
    /// 清除剑
    /// </summary>
    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSword);
        Destroy(sword);
    }

    /// <summary>
    /// 调用后先为真，过一段时间后，变为假
    /// </summary>
    /// <param name="_seconds"></param>
    /// <returns></returns>
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }

    /// <summary>
    /// 触发检测
    /// </summary>
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
   
    /// <summary>
    /// 冲刺
    /// </summary>
    private void CheckForDashInput()
    {
        if (IsWallDetected())
        {
            return;
        }

        if (skillManager.dash.dashUnlocked == false)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {

            dashDir = Input.GetAxisRaw("Horizontal");
            //如果冲刺方向为0，则往面朝方向冲刺
            if (dashDir == 0)
            {
                dashDir = facingDir;
            }

            stateMachine.ChangeState(dashState);
        }
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }
}
