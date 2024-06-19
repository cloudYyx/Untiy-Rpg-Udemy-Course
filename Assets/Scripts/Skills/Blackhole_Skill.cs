using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// �ڶ�����
/// </summary>
public class Blackhole_Skill : Skill
{
    //�����ڶ���ť
    [SerializeField] private UI_SkillTreeSlot blackHoleUnlockButton;
    //�Ƿ�����ڶ�
    public bool blackholeUnlocked { get; private set; }
    //�ڶ�Ԥ����
    [SerializeField] private GameObject blackHolePrefab;
    //������
    [SerializeField] private float maxSize;
    //�����ٶ�
    [SerializeField] private float growSpeed;
    //��С�ٶ�
    [SerializeField] private float shrinkSpeed;
    [Space]
    //��������
    [SerializeField] private int amountOfAttacks;
    //��¡������ȴʱ��
    [SerializeField] private float cloneCooldown;
    //�ڶ�����ʱ��
    [SerializeField] private float blackholeDuration;
    
    //�ڶ����ܿ�����
    Blackhole_Skill_Controller currentBlackhole;

    /// <summary>
    /// �����ڶ�
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
        //Object ʵ�����Ŀ�¡����
        //swordPrefab��Ҫ���Ƶ����ж���
        //position	�¶����λ�á�
        //rotation �¶���ķ���
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
    /// ���������
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
    /// ��ȡ�ڶ��뾶
    /// </summary>
    /// <returns></returns>
    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }
}
