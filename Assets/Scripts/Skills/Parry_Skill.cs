using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 格挡技能
/// </summary>
public class Parry_Skill : Skill
{
    [Header("Parry")]
    //格挡解锁按钮
    [SerializeField] private UI_SkillTreeSlot parryUnlockButton;
    //格挡是否解锁
    public bool parryUnlocked { get; private set; }

    [Header("Parry restore")]
    //恢复解锁按钮
    [SerializeField] private UI_SkillTreeSlot restoreUnlockButton;
    //恢复是否解锁
    public bool restoreUnlocked { get; private set; }
    [Range(0f, 1f)]
    //恢复生命值
    [SerializeField] private float restoreHealthAmount;

    [Header("Parry with mirage")]
    //幻影反击解锁按钮
    [SerializeField] private UI_SkillTreeSlot parryWithMirageUnlockButton;
    //幻影反击是否解锁
    public bool parryWithMirageUnlocked { get; private set; }


    public override void UseSkill()
    {
        base.UseSkill();

        if (restoreUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(player.characterState.GetMaxHealthValue() * restoreHealthAmount);
            player.characterState.IncreaseHealthBy(restoreAmount);
        }
    }

    protected override void Start()
    {
        base.Start();

        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
    }

    /// <summary>
    /// 解锁格挡
    /// </summary>
    private void UnlockParry()
    {
        if (parryUnlockButton.unlocked)
        {
            parryUnlocked = true;
        }
    }

    /// <summary>
    /// 解锁恢复
    /// </summary>
    private void UnlockParryRestore()
    {
        if (restoreUnlockButton.unlocked)
        {
            restoreUnlocked = true;
        }
    }

    /// <summary>
    /// 解锁幻影反击
    /// </summary>
    private void UnlockParryWithMirage()
    {
        if (parryWithMirageUnlockButton.unlocked)
        {
            parryWithMirageUnlocked = true;
        }
    }

    /// <summary>
    /// 格挡制造幻影
    /// </summary>
    /// <param name="_respawnTransform"></param>
    public void MakeMirageOnParry(Transform _respawnTransform)
    {
        if (parryWithMirageUnlocked)
        {
            SkillManager.instance.clone.CreateCloneWithDelay(_respawnTransform);
        }
    }
}
