using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
/// <summary>
/// 水晶技能控制器
/// </summary>
public class Crystal_Skill_Controller : MonoBehaviour
{
    //动画
    private Animator animator => GetComponent<Animator>();
    //圆形碰撞器
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    //玩家
    private Player player;

    //水晶存在定时器
    private float crystalExistTimer;

    //能否爆炸
    private bool canExplode;
    //能否移动
    private bool canMove;
    //移动速度
    private float moveSpeed;

    //能否生长
    private bool canGrow;
    //生长速度
    private float growSpeed = 5;

    //最近的目标
    private Transform closestTarget;
    //位码操作
    [SerializeField] private LayerMask whatIsEnemy;

    /// <summary>
    /// 设置水晶属性
    /// </summary>
    /// <param name="_crystalDuration"></param>
    public void SetupCrystal(float _crystalDuration,bool _canExplode,bool _canMove,float _moveSpeed,Transform _closestTarget, Player _player)
    {
        crystalExistTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestTarget = _closestTarget;
        player = _player;
    }

    /// <summary>
    /// 选择随机的敌人
    /// </summary>
    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackhole.GetBlackholeRadius();

        //获取位于圆形区域内的所有2D游戏碰撞体的列表  
        //第一参数是 point	圆形的中心。
        //第二参数是 radius	圆形的半径。
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);

        if (colliders.Length > 0)
        {
            closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
        }
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;

        if (crystalExistTimer < 0)
        {
            FinishCrystal();
        }

        if (canMove)
        {
            if (closestTarget != null)
            {
                //将属性 1 移向 属性2。
                transform.position = Vector2.MoveTowards(transform.position,
                    closestTarget.position, moveSpeed * Time.deltaTime);

                if (Vector2.Distance(transform.position, closestTarget.position) < 1)
                {
                    FinishCrystal();
                    canMove = false;
                }
            }
        }

        if (canGrow)
        {
            //相对于 GameObjects 父对象的变换缩放。
            //在向量 a 与 b 之间按 t 进行线性插值。
            //例如参数 t 限制在范围[0, 1] 内。
            //当 t = 0 时，返回 a 。
            //当 t = 1 时，返回 b 。
            //当 t = 0.5 时，返回 a 和 b 的中点。
            transform.localScale = Vector2.Lerp(transform.localScale,
                new Vector2(3, 3), growSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// 水晶爆炸事件
    /// </summary>
    private void AnimationExplodeEvent()
    {
        //获取位于圆形区域内的所有2D游戏碰撞体的列表  
        //第一参数是 point	圆形的中心。
        //第二参数是 radius	圆形的半径。
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);
        //遍历碰撞器包，检测是否存在敌人，如果存在，则调用敌人组件中的Damage函数
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                player.characterState.DoMagicalDamage(hit.GetComponent<CharacterStats>());
                //根据护身符加成添加特效
                ItemData_Equipment equipmentAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

                if (equipmentAmulet != null)
                {
                    equipmentAmulet.Effect(hit.transform);
                }
            }
        }
    }

    /// <summary>
    /// 水晶销毁
    /// </summary>
    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            animator.SetTrigger("Explode");
        }
        else
        {
            SelfDestroy();
        }
    }

    /// <summary>
    /// 自我毁灭
    /// </summary>
    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
