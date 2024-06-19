using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 克隆技能
/// </summary>
public class Clone_Skill : Skill
{
    [Header("Clone info")]
    //攻击乘数
    [SerializeField] private float attackMultiplier;
    //克隆的预制体
    [SerializeField] private GameObject clonePrefab;
    //克隆持续时间
    [SerializeField] private float cloneDuration;
    [Space]

    [Header("Clone attack")]
    //解锁克隆攻击按钮
    [SerializeField] private UI_SkillTreeSlot cloneAttackUnlockButton;
    //克隆攻击倍增器
    [SerializeField] private float cloneAttackMultiplier;
    //是否能攻击
    [SerializeField] private bool canAttack;

    [Header("Aggressive clone")]
    //解锁侵略性克隆按钮
    [SerializeField] private UI_SkillTreeSlot aggressiveCloneUnlockButton;
    //侵略性克隆攻击倍增器
    [SerializeField] private float aggressiveCloneAttackMultiplier;
    //可以应用命中效果吗
    public bool canApplyOnHitEffect { get; private set; }

    //多克隆
    [Header("Multiple clone")]
    //多重解锁按钮
    [SerializeField] private UI_SkillTreeSlot multipleUnlockButton;
    //多克隆攻击倍增器
    [SerializeField] private float multiCloneAttackMultiplier;
    //能否复制克隆
    [SerializeField] private bool canDuplicateClone;
    //复制的机会
    [SerializeField] private float chanceToDuplicate;

    //水晶代替克隆
    [Header("Crystal instead of clone")]
    //解锁水晶代替克隆按钮
    [SerializeField] private UI_SkillTreeSlot crystalInseadUnlockButton;
    //水晶是否代替克隆
    public bool crystalInseadOfClone;

    protected override void Start()
    {
        base.Start();

        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggressiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggressiveClone);
        multipleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
        crystalInseadUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInstead);
    }

    //解锁区域
    #region Unlock region
    /// <summary>
    /// 解锁克隆攻击
    /// </summary>
    private void UnlockCloneAttack()
    {
        if (cloneAttackUnlockButton.unlocked)
        {
            canAttack = true;
            attackMultiplier = cloneAttackMultiplier;
        }
    }

    /// <summary>
    /// 解锁侵略性克隆
    /// </summary>
    private void UnlockAggressiveClone()
    {
        if (aggressiveCloneUnlockButton.unlocked)
        {
            canApplyOnHitEffect = true;
            attackMultiplier = aggressiveCloneAttackMultiplier;
        }
    }

    /// <summary>
    /// 解锁多克隆
    /// </summary>
    private void UnlockMultiClone()
    {
        if (multipleUnlockButton.unlocked)
        {
            canDuplicateClone = true;
            attackMultiplier = multiCloneAttackMultiplier;
        }
    }

    /// <summary>
    /// 解锁水晶代替克隆
    /// </summary>
    private void UnlockCrystalInstead()
    {
        if (crystalInseadUnlockButton.unlocked)
        {
            crystalInseadOfClone = true;
        }
    }

    #endregion

    /// <summary>
    /// 创建克隆
    /// </summary>
    /// <param name="_clonePosition"></param>
    /// <param name="_offset"></param>
    public void CreateClone(Transform _clonePosition,Vector3 _offset)
    {
        if (crystalInseadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        //Object 实例化的克隆对象。   clonePrefab是要复制的现有对象。
        GameObject newClone = Instantiate(clonePrefab);
        //GetComponent 是访问游戏对象的组件的方法
        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(
            _clonePosition, cloneDuration,canAttack, _offset, FindClosestEnemy(newClone.transform),canDuplicateClone, chanceToDuplicate,player,attackMultiplier);
    }

    /// <summary>
    /// 在反击时创建克隆
    /// </summary>
    /// <param name="_enemyTransform"></param>
    public void CreateCloneWithDelay(Transform _enemyTransform)
    {
         StartCoroutine(CloneDelayCorotine(_enemyTransform, new Vector3(2 * player.facingDir, 0)));
    }

    /// <summary>
    /// 协程 创建延迟克隆
    /// </summary>
    /// <returns></returns>
    private IEnumerator CloneDelayCorotine(Transform _transform,Vector3 _offset)
    {
        yield return new WaitForSeconds(0.4f);
        CreateClone(_transform, _offset);
    }
}
