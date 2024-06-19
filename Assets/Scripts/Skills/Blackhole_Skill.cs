using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 黑洞技能
/// </summary>
public class Blackhole_Skill : Skill
{
    //解锁黑洞按钮
    [SerializeField] private UI_SkillTreeSlot blackHoleUnlockButton;
    //是否解锁黑洞
    public bool blackholeUnlocked { get; private set; }
    //黑洞预制体
    [SerializeField] private GameObject blackHolePrefab;
    //最大体积
    [SerializeField] private float maxSize;
    //生长速度
    [SerializeField] private float growSpeed;
    //缩小速度
    [SerializeField] private float shrinkSpeed;
    [Space]
    //攻击总数
    [SerializeField] private int amountOfAttacks;
    //克隆攻击冷却时间
    [SerializeField] private float cloneCooldown;
    //黑洞持续时间
    [SerializeField] private float blackholeDuration;
    
    //黑洞技能控制器
    Blackhole_Skill_Controller currentBlackhole;

    /// <summary>
    /// 解锁黑洞
    /// </summary>
    private void UnlockBlackhole()
    {
        if (blackHoleUnlockButton.unlocked)
        {
            blackholeUnlocked = true;
        }
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        //Object 实例化的克隆对象。
        //swordPrefab是要复制的现有对象。
        //position	新对象的位置。
        //rotation 新对象的方向。
        GameObject newBlackHole = Instantiate(blackHolePrefab,player.transform.position,Quaternion.identity);

        currentBlackhole = newBlackHole.GetComponent<Blackhole_Skill_Controller>();

        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneCooldown,blackholeDuration);
    }


    protected override void Start()
    {
        base.Start();

        blackHoleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackhole);
    }

    protected override void Update()
    {
        base.Update();
    }


    /// <summary>
    /// 技能完成了
    /// </summary>
    /// <returns></returns>
    public bool SkillCompleted()
    {
        if (!currentBlackhole)
        {
            return false;
        }

        if (currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 获取黑洞半径
    /// </summary>
    /// <returns></returns>
    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }
}
