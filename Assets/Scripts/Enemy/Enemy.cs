using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    //位码操作
    [SerializeField] protected LayerMask whatIsPlayer;

    //眩晕信息
    [Header("Stunned info")]
    //眩晕持续时间
    public float stunDuration;
    //眩晕方向
    public Vector2 stunDirection;
    //是否可以眩晕
    protected bool canBeStunned;
    //眩晕时机的图像
    [SerializeField] protected GameObject counterImage;

    [Header("Move info")]
    //移动速度
    public float moveSpeed;
    //空闲时间
    public float idleTime;
    //战斗时间
    public float battleTime;
    //默认移动速度
    private float defaultMoveSpeed;

    [Header("Attack info")]
    //攻击距离
    public float attackDistance;
    //攻击CD
    public float attackCooldown;
    //隐藏不出现在检查器里    最后攻击时间
    [HideInInspector]public float lastTimeAttacked;

    //状态机
    public EnemyStateMachine stateMachine { get; private set; }
    //最后的动画名字
    public string lastAnimBoolname {  get; private set; }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        
        defaultMoveSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    /// <summary>
    /// 分配最后一个动画名字
    /// </summary>
    /// <param name="_animBoolName"></param>
    public virtual void AssignLastAnimName(string _animBoolName)
    {
        lastAnimBoolname = _animBoolName;
    }

    /// <summary>
    /// 缓慢实体，寒冰元素会使角色缓慢
    /// </summary>
    /// <param name="_slowPercentage">缓慢百分比</param>
    /// <param name="_slowDuration">缓慢持续时间</param>
    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);

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
    }

    /// <summary>
    /// 冻结时间
    /// </summary>
    /// <param name="_timeFrozen"></param>
    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            animator.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            animator.speed = 1;
        }
    }

    /// <summary>
    /// 调用冻结时间
    /// </summary>
    /// <param name="_duration"></param>
    public virtual void FreezeTimeFor(float _duration)
    {
        StartCoroutine(FreezeTimerCoroutine(_duration));
    }

    /// <summary>
    /// 冻结定时器协同程序
    /// </summary>
    /// <param name="_seconds"></param>
    /// <returns></returns>
    protected virtual IEnumerator FreezeTimerCoroutine(float _seconds)
    {
        FreezeTime(true);

        yield return new WaitForSeconds(_seconds);

        FreezeTime(false);
    }

    #region Counter Attack Window
    /// <summary>
    /// 反击开关，可以反击 
    /// </summary>
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }
    /// <summary>
    /// 反击开关，不可以反击 
    /// </summary>
    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }

    /// <summary>
    /// 能否被眩晕
    /// </summary>
    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }
    #endregion
    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    /// <summary>
    /// 1.射线在 2D 空间中的起点。2.表示射线方向的矢量。3.射线的最大投射距离。4.过滤器，用于仅在特定层上检测碰撞体。
    /// </summary>
    /// <returns>返回关于光线投射2D物理检测到的对象的信息。</returns>
    public virtual RaycastHit2D IsPlayerDetected() =>
        Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir,
            15, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,
            new Vector3(transform.position.x + attackDistance * facingDir,
            transform.position.y));
    }
}
