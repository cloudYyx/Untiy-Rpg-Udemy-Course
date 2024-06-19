using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 冲刺技能
/// </summary>
public class Dash_Skill : Skill
{
    [Header("Dash")]
    //冲刺解锁按钮
    [SerializeField] private UI_SkillTreeSlot dashUnlockButton;
    //冲刺是否解锁
    public bool dashUnlocked { get; private set; }

    [Header("Clone on dash")]
    //冲刺克隆解锁按钮
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockButton;
    //冲刺克隆是否解锁
    public bool cloneOnDashUnlocked { get; private set; }

    [Header("Clone on arrival")]
    //冲刺到达克隆解锁按钮
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnlockButton;
    //冲刺到达克隆是否解锁
    public bool cloneOnArrivalUnlocked { get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void Start()
    {
        base.Start();

        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }

    /// <summary>
    /// 解锁冲刺
    /// </summary>
    private void UnlockDash()
    {
        if (dashUnlockButton.unlocked)
        {
            dashUnlocked = true;
        }
    }

    /// <summary>
    /// 解锁冲刺克隆
    /// </summary>
    private void UnlockCloneOnDash()
    {
        if (cloneOnDashUnlockButton.unlocked)
        {
            cloneOnDashUnlocked = true;
        }
    }

    /// <summary>
    /// 解锁冲刺到达克隆
    /// </summary>
    private void UnlockCloneOnArrival()
    {
        if (cloneOnArrivalUnlockButton.unlocked)
        {
            cloneOnArrivalUnlocked = true;
        }
    }

    /// <summary>
    /// 在冲刺开始时创建克隆
    /// </summary>
    public void CloneOnDash()
    {
        if (cloneOnDashUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }

    /// <summary>
    /// 在冲刺结束时创建克隆
    /// </summary>
    public void CloneOnArrival()
    {
        if (cloneOnArrivalUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }
}
