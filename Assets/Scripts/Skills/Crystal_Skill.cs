using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 水晶技能
/// </summary>
public class Crystal_Skill : Skill
{
    //水晶预制体
    [SerializeField] private GameObject crystalPrefab;
    //水晶持续时间
    [SerializeField] private float crystalDuration;
    //当前的水晶
    private GameObject currentCrystal;

    //水晶海市蜃楼
    [Header("Crystal mirage")]
    //解锁克隆代替水晶
    [SerializeField] private UI_SkillTreeSlot unlockCloneInsteadButton;
    //是否克隆代替水晶
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Crystal simple")]
    //解锁水晶按钮
    [SerializeField] private UI_SkillTreeSlot unlockCrystalButton;
    //是否解锁水晶
    public bool crystalUnlocked;

    //水晶爆炸
    [Header("Explosive crystal")]
    //解锁爆炸
    [SerializeField] private UI_SkillTreeSlot unlockExplosiveButton;
    //能否爆炸
    [SerializeField] private bool canExplode;

    [Header("Moving crystal")]
    //解锁向敌人移动
    [SerializeField] private UI_SkillTreeSlot unlockMovingCrystalButton;
    //能否向敌人移动
    [SerializeField] private bool canMoveToEnemy;
    //移动速度
    [SerializeField] private float moveSpeed;

    //多层水晶
    [Header("Multi stacking crystal")]
    //解锁多个堆叠
    [SerializeField] private UI_SkillTreeSlot unlockMultiStackButton;
    //可以使用多个堆叠
    [SerializeField] private bool canUseMultiStacks;
    //堆叠数量
    [SerializeField] private int amountOfStacks;
    //多堆叠冷却
    [SerializeField] private float multiStackCooldown;
    //时间窗口
    [SerializeField] private float useTimeWindow;
    //所有水晶
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

    protected override void Start()
    {
        base.Start();

        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockCloneInsteadButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
        unlockExplosiveButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);
        unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);
        unlockMultiStackButton.GetComponent<Button>().onClick.AddListener(UnlocMultiStack);
    }
    //解锁技能区域
    #region Unlock skill region
    /// <summary>
    /// 解锁水晶
    /// </summary>
    private void UnlockCrystal()
    {
        if (unlockCrystalButton.unlocked)
        {
            crystalUnlocked = true;
        }
    }

    /// <summary>
    /// 解锁水晶海市蜃楼
    /// </summary>
    private void UnlockCrystalMirage()
    {
        if (unlockCloneInsteadButton.unlocked)
        {
            cloneInsteadOfCrystal = true;
        }
    }

    /// <summary>
    /// 解锁水晶爆炸
    /// </summary>
    private void UnlockExplosiveCrystal()
    {
        if (unlockExplosiveButton.unlocked)
        {
            canExplode = true;
        }
    }

    /// <summary>
    /// 解锁水晶移动
    /// </summary>
    private void UnlockMovingCrystal()
    {
        if (unlockMovingCrystalButton.unlocked)
        {
            canMoveToEnemy = true;
        }
    }

    /// <summary>
    /// 解锁多层水晶
    /// </summary>
    private void UnlocMultiStack()
    {
        if (unlockMultiStackButton.unlocked)
        {
            canUseMultiStacks = true;
        }
    }
    #endregion

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())
        {
            return;
        }

        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMoveToEnemy)
            {
                return;
            }

            //玩家和水晶位置互换
            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            //克隆攻击或者水晶爆炸
            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
            }
        }
    }

    /// <summary>
    /// 创建水晶
    /// </summary>
    public void CreateCrystal()
    {
        //Object 实例化的克隆对象。
        //swordPrefab是要复制的现有对象。
        //position	新对象的位置。
        //rotation 新对象的方向。
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentCystalScrit = currentCrystal.GetComponent<Crystal_Skill_Controller>();

        currentCystalScrit.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform),player);
    }

    /// <summary>
    /// 当前水晶选择随机目标
    /// </summary>
    public void CurrentCrystalChooseRandomTarget()
    {
        currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy(); 
    }

    /// <summary>
    /// 可以使用多个水晶
    /// </summary>
    /// <returns></returns>
    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            if (crystalLeft.Count > 0)
            {
                //延迟调用补全水晶
                if (crystalLeft.Count == amountOfStacks)
                {
                    Invoke("ResetAbility", useTimeWindow);
                }

                //用完后补全水晶
                cooldown = 0;
                GameObject crystallToSpawn = crystalLeft[crystalLeft.Count - 1];
                //Object 实例化的克隆对象。
                //swordPrefab是要复制的现有对象。
                //position	新对象的位置。
                //rotation 新对象的方向。
                GameObject newCrystal = Instantiate(crystallToSpawn, player.transform.position, Quaternion.identity);

                crystalLeft.Remove(crystallToSpawn);

                newCrystal.GetComponent<Crystal_Skill_Controller>().
                    SetupCrystal(crystalDuration,canExplode,canMoveToEnemy,moveSpeed,
                    FindClosestEnemy(newCrystal.transform),player);

                if (crystalLeft.Count <= 0)
                {
                    cooldown = multiStackCooldown;
                    RefillCrystal();
                }
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 补充水晶
    /// </summary>
    private void RefillCrystal()
    {
        int amountToAdd = amountOfStacks - crystalLeft.Count;

        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    /// <summary>
    /// 复位能力
    /// </summary>
    private void ResetAbility()
    {
        if (cooldownTimer > 0)
        {
            return;
        }

        cooldownTimer = multiStackCooldown;
        RefillCrystal();
    }

    protected override void Update()
    {
        base.Update();
    }
}
