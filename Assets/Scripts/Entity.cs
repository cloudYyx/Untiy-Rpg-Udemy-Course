using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 实体
/// </summary>
public class Entity : MonoBehaviour
{
    #region Components
    //动画
    public Animator animator { get; private set; }
    //刚体
    public Rigidbody2D rb { get; private set; }
    //闪光效果
    public EntityFX entityFX { get; private set; }
    //渲染2D的精灵
    public SpriteRenderer spriteRenderer { get; private set; }
    //人物属性统计
    public CharacterStats characterState { get; private set; }
    //一个胶囊形状的原始对撞机
    public CapsuleCollider2D capsuleCollider2D { get; private set; }
    #endregion

    //击退信息
    [Header("Knockback info")]
    //击退方向
    [SerializeField] protected Vector2 knockbackDirection;
    //击退持续时间
    [SerializeField] protected float knockbackDuration;
    //是否击退
    protected bool isKnocked;

    //碰撞信息
    [Header("Collission info")]
    //攻击检查
    public Transform attackCheck;
    //攻击检查半径
    public float attackCheckRadius;
    //地面检查
    [SerializeField] protected Transform groundCheck;
    //地面检查距离
    [SerializeField] protected float groundCheckDistance;
    //墙面检查
    [SerializeField] protected Transform wallCheck;
    //墙面检查距离
    [SerializeField] protected float wallCheckDistance;
    //图层模板，什么是地面
    [SerializeField] protected LayerMask whatIsGround;

    //面朝方向
    public int facingDir { get; private set; } = 1;
    //面对右边
    protected bool facingRight = true;

    //Unity中使用Action进行简单的委托事件管理
    //可以用于Unity的事件处理，但是使用时要保证参数个数、类型、顺序必须完全⼀致。
    //在翻转的时候
    public System.Action onFlippeed;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        entityFX = GetComponent<EntityFX>();
        rb = GetComponent<Rigidbody2D>();
        characterState = GetComponent<CharacterStats>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update()
    {

    }

    /// <summary>
    /// 缓慢实体，寒冰元素会使角色缓慢
    /// </summary>
    /// <param name="_slowPercentage">缓慢百分比</param>
    /// <param name="_slowDuration">缓慢持续时间</param>
    public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        
    }

    /// <summary>
    /// 返回默认速度
    /// </summary>
    protected virtual void ReturnDefaultSpeed()
    {
        animator.speed = 1;
    }

    /// <summary>
    /// 受伤的影响
    /// </summary>
    public virtual void DamageImpact()
    {
        //启动一个名为“HitKnockback”的协程
        StartCoroutine("HitKnockback");
    }

    /// <summary>
    /// 击退  协程
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;

        rb.velocity = new Vector2(knockbackDirection.x * -facingDir,knockbackDirection.y);

        //等待击退持续时间结束，一段指定的时间延迟之后继续执行，在所有的Update函数完成调用的那一帧之后（这里的时间会受到Time.timeScale的影响）;
        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
    }

    #region Velocity
    /// <summary>
    /// 速度变为0
    /// </summary>
    public void SetZeroVelocity()
    {
        if (isKnocked)
        {
            return;
        }

        rb.velocity = new Vector2(0, 0);
    }
    //移动玩家
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
        {
            return;
        }

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion

    #region Collision
    /// <summary>
    /// 1.射线在 2D 空间中的起点。2.表示射线方向的矢量。3.射线的最大投射距离。4.过滤器，用于仅在特定层上检测碰撞体。
    /// </summary>
    /// <returns>返回关于光线投射2D物理检测到的对象的信息。</returns>
    //地面检测
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position,
        Vector2.down, groundCheckDistance, whatIsGround);
    //墙面检测
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position,
        Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    /// <summary>
    /// 画图功能
    /// </summary>
    protected virtual void OnDrawGizmos()
    {
        //从地面检查点划线，垂直检查划线
        Gizmos.DrawLine(groundCheck.position,
            new Vector3(groundCheck.position.x,
            groundCheck.position.y - groundCheckDistance));
        //从墙面检查点划线，水平检查划线
        Gizmos.DrawLine(wallCheck.position,
            new Vector3(wallCheck.position.x + wallCheckDistance,
            wallCheck.position.y));
        //绘制具有中心和半径的线框球体,攻击检查坐标体的位置和攻击检查半径
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    #region Flip
    //方向切换器 facingDir = 1 ，facingRight = true 是右边 反之亦然
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        //人物根据y轴180旋转
        transform.Rotate(0, 180, 0);
        
        if (onFlippeed != null)
        {
            onFlippeed();
        }
    }

    //控制方向的方法
    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
        {
            Flip();
        }
        else if (_x < 0 && facingRight)
        {
            Flip();
        }
    }
    #endregion

    /// <summary>
    /// 死亡
    /// </summary>
    public virtual void Die()
    {

    }
}
