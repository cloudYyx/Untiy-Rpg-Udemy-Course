using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
/// <summary>
/// 剑技能控制器
/// </summary>
public class Sword_Skill_Controller : MonoBehaviour
{
    //动画师
    private Animator animator;
    //刚体
    private Rigidbody2D rb;
    //圆形碰撞器
    private CircleCollider2D circleCollider2D;
    //玩家
    private Player player;
    //能否旋转
    private bool canRotate = true;
    //能否返回
    private bool isReturning;

    //冻结持续时间
    private float freezeTimeDuration;
    //返回速度
    private float returnSpeed = 12;

    [Header("Pierce info")]
    //最大穿刺量
    private float pierceAmount;

    [Header("Bounce info")]
    //是否在跳跃
    private bool isBouncing;
    //最大跳跃量
    private int bounceAmount;
    //所有敌人目标
    private List<Transform> enemyTarget;
    //目标指数
    private int targetIndex;
    //弹跳速度
    private float bounceSpeed;

    [Header("Spin info")]
    //最远距离
    private float maxTravelDistance;
    //旋转持续时间
    private float spinDuration;
    //旋转定时器
    private float spinTimer;
    //是否停止
    private bool wasStopped;
    //是否在旋转
    private bool isSpinning;
    //旋转方向
    private float spinDirection;

    //命中定时器
    private float hitTimer;
    //命中冷却
    private float hitCooldown;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    /// <summary>
    /// 摧毁
    /// </summary>
    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    //设置剑参数 (方向，重力刻度)
    public void SetupSword(Vector2 _dir,float _gravityScale,Player _player,float _freezeTimeDuration,float _returnSpeed)
    {
        player = _player;
        freezeTimeDuration = _freezeTimeDuration;
        returnSpeed = _returnSpeed;
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;

        if (pierceAmount <= 0)
        {
            animator.SetBool("Rotation", true);
        }
        //在给定的最小浮点数值和最大浮点数值之间钳制给定值。如果给定值在最小值和最大值范围内，则返回给定值。
        //如果给定的浮点值小于最小值，则返回最小值。
        //如果给定值大于最大值，则返回最大值。使用“夹具”将值限制为由最小值和最大值定义的范围。
        //如果最小值大于最大值，则返回未定义的值。
        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        Invoke("DestroyMe", 7);
    }

    //设置剑跳跃参数（是否在跳跃，最大跳跃量）
    public void SetupBounce(bool _isBouncing, int _amountOfBounce,float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountOfBounce;
        bounceSpeed = _bounceSpeed;

        enemyTarget = new List<Transform>();
    }

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetupSpin(bool _isSpinning,float _maxTravelDistance,float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }

    /// <summary>
    /// 返回剑
    /// </summary>
    public void ReturnSword()
    {
        //冻结所有轴旋转和运动
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //这个刚体应该脱离物理控制吗？
        //rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }

    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.velocity;
        }

        if (isReturning)
        {
            //将点 属性1 移向 属性2。
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.transform.position,
                returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.CatchTheSword();
            }
        }

        BounceLogic();
        SpinLogic();
    }

    /// <summary>
    /// 旋转逻辑
    /// </summary>
    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;
                //将点 属性1 移向 属性2
                //transform.position = Vector2.MoveTowards(transform.position,
                //    new Vector2(transform.position.x + spinDirection,
                //    transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    //若进入触发器，则生成一个范围
                    //获取位于圆形区域内的所有2D游戏碰撞体的列表  
                    //第一参数是 point	圆形的中心。
                    //第二参数是 radius	圆形的半径。
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                        {
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 停止旋转
    /// </summary>
    private void StopWhenSpinning()
    {
        wasStopped = true;
        //冻结X和Y的运动
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }
    
    /// <summary>
    /// 弹跳逻辑
    /// </summary>
    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            //将点 属性1 移向 属性2
            transform.position = Vector2.MoveTowards(
                transform.position, enemyTarget[targetIndex].position,
                bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < 0.1f)
            {
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());

                targetIndex++;
                bounceAmount--;

                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }

    //如果另一个碰撞器进入触发器，则调用这个
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
        {
            return;
        }

        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }

        SetupTargetForBounce(collision);

        StuckInto(collision);
    }

    /// <summary>
    /// 用剑技能伤害敌人
    /// </summary>
    /// <param name="enemy"></param>
    private void SwordSkillDamage(Enemy enemy)
    {
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
        player.characterState.DoDamage(enemyStats);

        if (player.skillManager.sword.timeStopUnlocked)
        {
            enemy.FreezeTimeFor(freezeTimeDuration);
        }

        if (player.skillManager.sword.vulnerabilityUnlocked)
        {
            enemyStats.MakeVulnerableFor(freezeTimeDuration);
        }

        //根据护身符加成添加特效
        ItemData_Equipment equipmentAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

        if (equipmentAmulet != null)
        {
            equipmentAmulet.Effect(enemy.transform);
        }
    }

    //为弹跳设置目标
    private void SetupTargetForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                //若进入触发器，则生成一个范围
                //获取位于圆形区域内的所有2D游戏碰撞体的列表  
                //第一参数是 point	圆形的中心。
                //第二参数是 radius	圆形的半径。
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        //若范围里有敌人，则添加进敌人目标集合里
                        enemyTarget.Add(hit.transform);
                    }
                }
            }
        }
    }

    //卡入
    private void StuckInto(Collider2D collision)
    {
        if(pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        canRotate = false;
        //关闭碰撞器
        circleCollider2D.enabled = false;
        //这个刚体应该脱离物理控制吗？
        rb.isKinematic = true;
        //控制该 Rigidbody2D 的模拟自由度 = 沿所有轴冻结旋转和移动。
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        if (isBouncing && enemyTarget.Count > 0)
        {
            return;
        }

        animator.SetBool("Rotation", false);
        //让它成为被碰撞物理的子体    
        transform.parent = collision.transform;
    }
}
