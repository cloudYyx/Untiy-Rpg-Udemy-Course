using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// �񵲼���
/// </summary>
public class Parry_Skill : Skill
{
    [Header("Parry")]
    //�񵲽�����ť
    [SerializeField] private UI_SkillTreeSlot parryUnlockButton;
    //���Ƿ����
    public bool parryUnlocked { get; private set; }

    [Header("Parry restore")]
    //�ָ�������ť
    [SerializeField] private UI_SkillTreeSlot restoreUnlockButton;
    //�ָ��Ƿ����
    public bool restoreUnlocked { get; private set; }
    [Range(0f, 1f)]
    //�ָ�����ֵ
    [SerializeField] private float restoreHealthAmount;

    [Header("Parry with mirage")]
    //��Ӱ����������ť
    [SerializeField] private UI_SkillTreeSlot parryWithMirageUnlockButton;
    //��Ӱ�����Ƿ����
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
    /// ������
    /// </summary>
    private void UnlockParry()
    {
        if (parryUnlockButton.unlocked)
        {
            parryUnlocked = true;
        }
    }

    /// <summary>
    /// �����ָ�
    /// </summary>
    private void UnlockParryRestore()
    {
        if (restoreUnlockButton.unlocked)
        {
            restoreUnlocked = true;
        }
    }

    /// <summary>
    /// ������Ӱ����
    /// </summary>
    private void UnlockParryWithMirage()
    {
        if (parryWithMirageUnlockButton.unlocked)
        {
            parryWithMirageUnlocked = true;
        }
    }

    /// <summary>
    /// �������Ӱ
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
