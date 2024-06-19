using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ��¡����
/// </summary>
public class Clone_Skill : Skill
{
    [Header("Clone info")]
    //��������
    [SerializeField] private float attackMultiplier;
    //��¡��Ԥ����
    [SerializeField] private GameObject clonePrefab;
    //��¡����ʱ��
    [SerializeField] private float cloneDuration;
    [Space]

    [Header("Clone attack")]
    //������¡������ť
    [SerializeField] private UI_SkillTreeSlot cloneAttackUnlockButton;
    //��¡����������
    [SerializeField] private float cloneAttackMultiplier;
    //�Ƿ��ܹ���
    [SerializeField] private bool canAttack;

    [Header("Aggressive clone")]
    //���������Կ�¡��ť
    [SerializeField] private UI_SkillTreeSlot aggressiveCloneUnlockButton;
    //�����Կ�¡����������
    [SerializeField] private float aggressiveCloneAttackMultiplier;
    //����Ӧ������Ч����
    public bool canApplyOnHitEffect { get; private set; }

    //���¡
    [Header("Multiple clone")]
    //���ؽ�����ť
    [SerializeField] private UI_SkillTreeSlot multipleUnlockButton;
    //���¡����������
    [SerializeField] private float multiCloneAttackMultiplier;
    //�ܷ��ƿ�¡
    [SerializeField] private bool canDuplicateClone;
    //���ƵĻ���
    [SerializeField] private float chanceToDuplicate;

    //ˮ�������¡
    [Header("Crystal instead of clone")]
    //����ˮ�������¡��ť
    [SerializeField] private UI_SkillTreeSlot crystalInseadUnlockButton;
    //ˮ���Ƿ�����¡
    public bool crystalInseadOfClone;

    protected override void Start()
    {
        base.Start();

        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggressiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggressiveClone);
        multipleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
        crystalInseadUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInstead);
    }

    //��������
    #region Unlock region
    /// <summary>
    /// ������¡����
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
    /// ���������Կ�¡
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
    /// �������¡
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
    /// ����ˮ�������¡
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
    /// ������¡
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

        //Object ʵ�����Ŀ�¡����   clonePrefab��Ҫ���Ƶ����ж���
        GameObject newClone = Instantiate(clonePrefab);
        //GetComponent �Ƿ�����Ϸ���������ķ���
        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(
            _clonePosition, cloneDuration,canAttack, _offset, FindClosestEnemy(newClone.transform),canDuplicateClone, chanceToDuplicate,player,attackMultiplier);
    }

    /// <summary>
    /// �ڷ���ʱ������¡
    /// </summary>
    /// <param name="_enemyTransform"></param>
    public void CreateCloneWithDelay(Transform _enemyTransform)
    {
         StartCoroutine(CloneDelayCorotine(_enemyTransform, new Vector3(2 * player.facingDir, 0)));
    }

    /// <summary>
    /// Э�� �����ӳٿ�¡
    /// </summary>
    /// <returns></returns>
    private IEnumerator CloneDelayCorotine(Transform _transform,Vector3 _offset)
    {
        yield return new WaitForSeconds(0.4f);
        CreateClone(_transform, _offset);
    }
}
