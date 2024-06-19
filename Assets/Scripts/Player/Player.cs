using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
/// <summary>
/// ���
/// </summary>
public class Player : Entity
{
    [Header("Attack details")]
    //��������
    public Vector2[] attackMovement;
    //��������ʱ��
    public float counterAttackDuration = 0.2f;

    //����Ƿ���æ
    public bool isBusy {  get; private set; }
    [Header("Move info")]
    //�ƶ��ٶ�
    public float moveSpeed = 12f;
    //��Ծ��
    public float jumpForce;
    //������Ӱ��
    public float swordReturnImpact;
    //Ĭ���ƶ��ٶ�
    private float defaultMoveSpeed;
    //Ĭ����Ծ��
    private float defaultJumpForce;

    [Header("Dash info")]
    //�����
    public float dashSpeed;
    //��̳���ʱ��
    public float dashDuration;
    //��̷���
    public float dashDir { get; private set; }
    //Ĭ�ϳ���ٶ�
    private float defaultDashSpeed;


    //���ܹ�����
    public SkillManager skillManager { get; private set; }
    //��
    public GameObject sword { get; private set; }

    #region States
    //��ȡ�ǹ��У��޸���˽�� == ֻ��
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
    /// ����ʵ�壬����Ԫ�ػ�ʹ��ɫ����
    /// </summary>
    /// <param name="_slowPercentage">�����ٷֱ�</param>
    /// <param name="_slowDuration">��������ʱ��</param>
    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);

        animator.speed = animator.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    /// <summary>
    /// ����Ĭ���ٶ�
    /// </summary>
    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    /// <summary>
    /// �����µĽ�
    /// </summary>
    /// <param name="_newSword"></param>
    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    /// <summary>
    /// �����
    /// </summary>
    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSword);
        Destroy(sword);
    }

    /// <summary>
    /// ���ú���Ϊ�棬��һ��ʱ��󣬱�Ϊ��
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
    /// �������
    /// </summary>
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
   
    /// <summary>
    /// ���
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
            //�����̷���Ϊ0�������泯������
            if (dashDir == 0)
            {
                dashDir = facingDir;
            }

            stateMachine.ChangeState(dashState);
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }
}
