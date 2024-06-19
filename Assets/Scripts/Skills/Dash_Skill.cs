using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ��̼���
/// </summary>
public class Dash_Skill : Skill
{
    [Header("Dash")]
    //��̽�����ť
    [SerializeField] private UI_SkillTreeSlot dashUnlockButton;
    //����Ƿ����
    public bool dashUnlocked { get; private set; }

    [Header("Clone on dash")]
    //��̿�¡������ť
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockButton;
    //��̿�¡�Ƿ����
    public bool cloneOnDashUnlocked { get; private set; }

    [Header("Clone on arrival")]
    //��̵����¡������ť
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnlockButton;
    //��̵����¡�Ƿ����
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
    /// �������
    /// </summary>
    private void UnlockDash()
    {
        if (dashUnlockButton.unlocked)
        {
            dashUnlocked = true;
        }
    }

    /// <summary>
    /// ������̿�¡
    /// </summary>
    private void UnlockCloneOnDash()
    {
        if (cloneOnDashUnlockButton.unlocked)
        {
            cloneOnDashUnlocked = true;
        }
    }

    /// <summary>
    /// ������̵����¡
    /// </summary>
    private void UnlockCloneOnArrival()
    {
        if (cloneOnArrivalUnlockButton.unlocked)
        {
            cloneOnArrivalUnlocked = true;
        }
    }

    /// <summary>
    /// �ڳ�̿�ʼʱ������¡
    /// </summary>
    public void CloneOnDash()
    {
        if (cloneOnDashUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }

    /// <summary>
    /// �ڳ�̽���ʱ������¡
    /// </summary>
    public void CloneOnArrival()
    {
        if (cloneOnArrivalUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }
}
