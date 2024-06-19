using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ���㼼��
/// </summary>
public class Dodge_Skill : Skill
{
    [Header("Dodge")]
    //�������㰴ť
    [SerializeField] private UI_SkillTreeSlot unlockDodgeButton;
    //����ֵ
    [SerializeField] private int evasionAmount;
    //�Ƿ��������
    public bool dodgeUnlocked;

    [Header("Mirage dodge")]
    //������Ӱ������ť
    [SerializeField] private UI_SkillTreeSlot unlockMirageDodgeButton;
    //�Ƿ������Ӱ����
    public bool dodgeMirageUnlocked;

    protected override void Start()
    {
        base.Start();

        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }

    /// <summary>
    /// ��������
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
    /// ������Ӱ����
    /// </summary>
    private void UnlockMirageDodge()
    {
        if (unlockMirageDodgeButton.unlocked)
        {
            dodgeMirageUnlocked = true;
        }
    }

    /// <summary>
    /// ������ʱ�����Ӱ
    /// </summary>
    public void CreateMirageOnDoDodge()
    {
        if (dodgeMirageUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, new Vector3(2 * player.facingDir,0));
        }
    }
}
