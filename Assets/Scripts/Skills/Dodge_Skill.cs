using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 闪躲技能
/// </summary>
public class Dodge_Skill : Skill
{
    [Header("Dodge")]
    //解锁闪躲按钮
    [SerializeField] private UI_SkillTreeSlot unlockDodgeButton;
    //闪避值
    [SerializeField] private int evasionAmount;
    //是否解锁闪躲
    public bool dodgeUnlocked;

    [Header("Mirage dodge")]
    //解锁幻影躲闪按钮
    [SerializeField] private UI_SkillTreeSlot unlockMirageDodgeButton;
    //是否解锁幻影躲闪
    public bool dodgeMirageUnlocked;

    protected override void Start()
    {
        base.Start();

        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }

    /// <summary>
    /// 解锁闪躲
    /// </summary>
    private void UnlockDodge()
    {
        if (unlockDodgeButton.unlocked)
        {
            player.characterState.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatsUI();
            dodgeUnlocked = true;
        }
    }

    /// <summary>
    /// 解锁幻影躲闪
    /// </summary>
    private void UnlockMirageDodge()
    {
        if (unlockMirageDodgeButton.unlocked)
        {
            dodgeMirageUnlocked = true;
        }
    }

    /// <summary>
    /// 在闪避时创造幻影
    /// </summary>
    public void CreateMirageOnDoDodge()
    {
        if (dodgeMirageUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, new Vector3(2 * player.facingDir,0));
        }
    }
}
