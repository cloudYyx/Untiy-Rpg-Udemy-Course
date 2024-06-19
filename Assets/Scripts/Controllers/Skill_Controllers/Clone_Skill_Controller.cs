using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 克隆技能控制器
/// </summary>
public class Clone_Skill_Controller : MonoBehaviour
{
    //玩家
    private Player player;
    //渲染2D图形的精灵
    private SpriteRenderer spriteRenderer;
    //动画师
    private Animator animator;
    //颜色失去的时间
    [SerializeField] private float colorLoosingSpeed;

    //克隆定时器
    private float cloneTimer;
    //攻击乘数
    private float attackMultiplier;
    //攻击检查
    [SerializeField] private Transform attackCheck;
    //攻击检查半径
    [SerializeField] private float attackCheckRadius = 0.8f;
    //最近的怪物
    private Transform closestEnemy;
    //面朝向 1为右 -1为左
    private int facingDir = 1;

    //能否复制克隆
    private bool canDuplicateClone;
    //复制的机会
    private float chanceToduplicate;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            //第四个参数是透明度
            spriteRenderer.color = new Color(1, 1, 1,
                spriteRenderer.color.a - (Time.deltaTime * colorLoosingSpeed));
            if (spriteRenderer.color.a <= 0)
            {
                //移除游戏对象
                Destroy(gameObject);
            }
        }
    }

    //设置预制体克隆位置,_offset偏移量
    public void SetupClone(Transform _newTransform,float _cloneDuration,bool _canAttack,Vector3 _offset,Transform _closestEnemy,bool _canDuplicate,float _chanceToduplicate,Player _player,float _attackMultiplier)
    {
        if (_canAttack)
        {
            animator.SetInteger("AttackNumber", Random.Range(1, 3));
        }

        attackMultiplier = _attackMultiplier;
        player = _player;
        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;

        closestEnemy = _closestEnemy;
        canDuplicateClone = _canDuplicate;
        chanceToduplicate = _chanceToduplicate;

        FaceClosesTarget();
    }

    /// <summary>
    /// 动画触发器方法
    /// 在动画窗口-样本右边-添加事件里调用此方法可以让触发器变为ture
    /// </summary>
    private void AnimationTrigger()
    {
        cloneTimer = -0.1f;
    }

    /// <summary>
    /// 动画触发器方法
    /// 在动画窗口-样本右边-添加事件里调用此方法可以让触发器变为ture
    /// </summary>
    private void AttackTrigger()
    {
        //获取位于圆形区域内的所有2D游戏碰撞体的列表  
        //第一参数是 point	圆形的中心。
        //第二参数是 radius	圆形的半径。
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
        //遍历碰撞器包，检测是否存在敌人，如果存在，则调用敌人组件中的Damage函数
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                //player.characterState.DoDamage(hit.GetComponent<CharacterStats>());
                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();

                playerStats.CloneDoDamage(enemyStats, attackMultiplier);

                if (player.skillManager.clone.canApplyOnHitEffect)
                {
                    ItemData_Equipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);

                    if (weaponData != null)
                    {
                        weaponData.Effect(hit.transform);
                    }
                }

                if (canDuplicateClone)
                {
                    if (Random.Range(0,100) < chanceToduplicate)
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(0.5f * facingDir, 0));
                    }
                }
            }
        }
    }

    /// <summary>
    /// 面朝最近的目标
    /// </summary>
    private void FaceClosesTarget()
    {
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
