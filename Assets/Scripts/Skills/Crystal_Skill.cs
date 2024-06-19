using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ˮ������
/// </summary>
public class Crystal_Skill : Skill
{
    //ˮ��Ԥ����
    [SerializeField] private GameObject crystalPrefab;
    //ˮ������ʱ��
    [SerializeField] private float crystalDuration;
    //��ǰ��ˮ��
    private GameObject currentCrystal;

    //ˮ��������¥
    [Header("Crystal mirage")]
    //������¡����ˮ��
    [SerializeField] private UI_SkillTreeSlot unlockCloneInsteadButton;
    //�Ƿ��¡����ˮ��
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Crystal simple")]
    //����ˮ����ť
    [SerializeField] private UI_SkillTreeSlot unlockCrystalButton;
    //�Ƿ����ˮ��
    public bool crystalUnlocked;

    //ˮ����ը
    [Header("Explosive crystal")]
    //������ը
    [SerializeField] private UI_SkillTreeSlot unlockExplosiveButton;
    //�ܷ�ը
    [SerializeField] private bool canExplode;

    [Header("Moving crystal")]
    //����������ƶ�
    [SerializeField] private UI_SkillTreeSlot unlockMovingCrystalButton;
    //�ܷ�������ƶ�
    [SerializeField] private bool canMoveToEnemy;
    //�ƶ��ٶ�
    [SerializeField] private float moveSpeed;

    //���ˮ��
    [Header("Multi stacking crystal")]
    //��������ѵ�
    [SerializeField] private UI_SkillTreeSlot unlockMultiStackButton;
    //����ʹ�ö���ѵ�
    [SerializeField] private bool canUseMultiStacks;
    //�ѵ�����
    [SerializeField] private int amountOfStacks;
    //��ѵ���ȴ
    [SerializeField] private float multiStackCooldown;
    //ʱ�䴰��
    [SerializeField] private float useTimeWindow;
    //����ˮ��
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
    //������������
    #region Unlock skill region
    /// <summary>
    /// ����ˮ��
    /// </summary>
    private void UnlockCrystal()
    {
        if (unlockCrystalButton.unlocked)
        {
            crystalUnlocked = true;
        }
    }

    /// <summary>
    /// ����ˮ��������¥
    /// </summary>
    private void UnlockCrystalMirage()
    {
        if (unlockCloneInsteadButton.unlocked)
        {
            cloneInsteadOfCrystal = true;
        }
    }

    /// <summary>
    /// ����ˮ����ը
    /// </summary>
    private void UnlockExplosiveCrystal()
    {
        if (unlockExplosiveButton.unlocked)
        {
            canExplode = true;
        }
    }

    /// <summary>
    /// ����ˮ���ƶ�
    /// </summary>
    private void UnlockMovingCrystal()
    {
        if (unlockMovingCrystalButton.unlocked)
        {
            canMoveToEnemy = true;
        }
    }

    /// <summary>
    /// �������ˮ��
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

            //��Һ�ˮ��λ�û���
            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            //��¡��������ˮ����ը
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
    /// ����ˮ��
    /// </summary>
    public void CreateCrystal()
    {
        //Object ʵ�����Ŀ�¡����
        //swordPrefab��Ҫ���Ƶ����ж���
        //position	�¶����λ�á�
        //rotation �¶���ķ���
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentCystalScrit = currentCrystal.GetComponent<Crystal_Skill_Controller>();

        currentCystalScrit.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform),player);
    }

    /// <summary>
    /// ��ǰˮ��ѡ�����Ŀ��
    /// </summary>
    public void CurrentCrystalChooseRandomTarget()
    {
        currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy(); 
    }

    /// <summary>
    /// ����ʹ�ö��ˮ��
    /// </summary>
    /// <returns></returns>
    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            if (crystalLeft.Count > 0)
            {
                //�ӳٵ��ò�ȫˮ��
                if (crystalLeft.Count == amountOfStacks)
                {
                    Invoke("ResetAbility", useTimeWindow);
                }

                //�����ȫˮ��
                cooldown = 0;
                GameObject crystallToSpawn = crystalLeft[crystalLeft.Count - 1];
                //Object ʵ�����Ŀ�¡����
                //swordPrefab��Ҫ���Ƶ����ж���
                //position	�¶����λ�á�
                //rotation �¶���ķ���
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
    /// ����ˮ��
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
    /// ��λ����
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
