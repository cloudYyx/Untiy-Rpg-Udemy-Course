using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;
/// <summary>
/// 黑洞技能控制器
/// </summary>
public class Blackhole_Skill_Controller : MonoBehaviour
{
    //热键预制体
    [SerializeField] private GameObject hotKeyPrefab;
    //所有输入
    [SerializeField] private List<KeyCode> keyCodeList;

    //最大体积
    private float maxSize;
    //生长速度
    private float growSpeed;
    //缩小速度
    private float shrinkSpeed;
    //黑洞定时器
    private float blackholeTimer;

    //是否生长
    private bool canGrow = true;
    //能否缩小
    private bool canShrink;
    //玩家是否隐身
    private bool playerCanDisapear = true;
    //能否创建热键o
    private bool canCreateHotKeys = true;
    //克隆攻击是否释放
    private bool cloneAttackReleased;

    //攻击总数
    private int amountOfAttacks = 4;
    //克隆攻击冷却时间
    private float cloneAttackCooldown = 0.3f;
    //克隆攻击定时器
    private float cloneAttackTimer;

    //所有笼罩的目标
    private List<Transform> targets = new List<Transform>();
    //所有热键
    private List<GameObject> createdHotKey = new List<GameObject>();

    //玩家可以退出状态
    public bool playerCanExitState { get;private set; }

    /// <summary>
    /// 设置黑洞属性
    /// 最大体积,生长速度,缩小速度,攻击总数,克隆攻击冷却时间
    /// </summary>
    public void SetupBlackhole(float _maxSize,float _growSpeed,float _shrinkSpeed,int _amountOfAttacks,float _cloneAttackCooldown,float _blackholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackholeTimer = _blackholeDuration;

        if (SkillManager.instance.clone.crystalInseadOfClone)
        {
            playerCanDisapear = false;
        }
    }

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if (blackholeTimer < 0)
        {
            blackholeTimer = Mathf.Infinity;

            if (targets.Count > 0)
            {
                ReleaseCloneAttack();
            }
            else
            {
                FinishBlackHoleAbility();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            //相对于 GameObjects 父对象的变换缩放。
            //在向量 a 与 b 之间按 t 进行线性插值。
            //例如参数 t 限制在范围[0, 1] 内。
            //当 t = 0 时，返回 a 。
            //当 t = 1 时，返回 b 。
            //当 t = 0.5 时，返回 a 和 b 的中点。
            transform.localScale = Vector2.Lerp(transform.localScale,
                new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale,
                new Vector2(-1, 1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
            {
                canShrink = false;
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// 释放克隆攻击
    /// </summary>
    private void ReleaseCloneAttack()
    {
        if (targets.Count < 0)
        {
            return;
        }

        DestroyHotKeys();
        cloneAttackReleased = true;
        canCreateHotKeys = false;

        if (playerCanDisapear)
        {
            playerCanDisapear = false;
            PlayerManager.instance.player.entityFX.MakeTransprent(true);
        }
        
    }

    /// <summary>
    /// 克隆攻击逻辑
    /// </summary>
    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased && amountOfAttacks > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);

            float xOffset;

            if (Random.Range(0, 100) > 50)
            {
                xOffset = 2;
            }
            else
            {
                xOffset = -2;
            }

            if (SkillManager.instance.clone.crystalInseadOfClone)
            {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            }
            else if(amountOfAttacks > 0)
            {
                SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));

            }

            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {                
                Invoke("FinishBlackHoleAbility", 0.5f);
            }
        }
    }

    /// <summary>
    /// 完成黑洞技能
    /// </summary>
    private void FinishBlackHoleAbility()
    {
        DestroyHotKeys();
        playerCanExitState = true;
        canShrink = true;
        cloneAttackReleased = false;
    }

    /// <summary>
    /// 删除热键
    /// </summary>
    private void DestroyHotKeys()
    {
        if (createdHotKey.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < createdHotKey.Count; i++)
        {
            Destroy(createdHotKey[i]);
        }
    }

    /// <summary>
    //如果另一个碰撞器进入触发器，则调用这个
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            
            CreateHotKey(collision);
        }
    }

    /// <summary>
    //如果另一个碰撞器退出触发器，则调用这个
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(false);
        }
    }
    //简写，等价上面的方法
    //private void OnTriggerExit(Collider collision) => collision.GetComponent<Enemy>()?.FreezeTime(false);


    /// <summary>
    /// 制造热键
    /// </summary>
    /// <param name="collision"></param>
    private void CreateHotKey(Collider2D collision)
    {
        if(keyCodeList.Count <= 0)
        {
            return;
        }

        if (!canCreateHotKeys)
        {
            return;
        }

        //实例化 
        //original 要复制的现有对象。
        //position 新对象的位置。
        //rotation 新对象的方向。
        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKey.Add(newHotKey);

        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);

        Blackhole_HotKey_Skill_Controller newHotKeyScript = newHotKey.GetComponent<Blackhole_HotKey_Skill_Controller>();

        newHotKeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform)
    {
        targets.Add(_enemyTransform);
    }
}
