
using System.Collections;
using UnityEngine;

/// <summary>
/// ��������
/// </summary>
public enum StatType
{
    strength,//����
    agility,//����
    intelligence,//����
    vitality,//����

    maxHealth,//���Ѫ��
    armor,//����
    evasion,//������
    magicResistance,//ħ������

    damage,//�˺�
    critChance,//������
    critPower,//��������

    fireDamage,//�����˺�
    iceDamage,//��˪�˺�
    lightningDamage//�׵��˺�
}

/// <summary>
/// ��������
/// </summary>
public class CharacterStats : MonoBehaviour
{
    //����Ч��
    private EntityFX entityFX;

    //��Ҫ����
    [Header("Major stats")]
    //���� 1������1���˺���1%��������
    public Stat strength;
    //���� 1������1%�����ʺ�1%��������
    public Stat agility;
    //���� 1������1��ħ���˺���3��ħ������
    public Stat intelligence;
    //���� 1������5������ֵ
    public Stat vitality;

    //��������
    [Header("Defensive stats")]
    //���Ѫ��
    public Stat maxHealth;
    //����
    public Stat armor;
    //������
    public Stat evasion;
    //ħ������
    public Stat magicResistance;

    //��������
    [Header("Offensive stats")]
    //�˺�
    public Stat damage;
    //������
    public Stat critChance;
    //�������� Ĭ�� 150%
    public Stat critPower;

    //ħ������
    [Header("Magic stats")]
    //�����˺�
    public Stat fireDamage;
    //��˪�˺�
    public Stat iceDamage;
    //�׵��˺�
    public Stat lightningDamage;

    //�Ƿ񱻵�ȼ �˺�����ʱ�������
    public bool isIgnited;
    //�Ƿ񱻱��� ����20%�Ļ���
    public bool isChilled;
    //�Ƿ��� ����׼ȷ��20%
    public bool isShocked;

    //Ԫ�س���ʱ��
    [SerializeField] private float elementsDuartion = 4;
    //��ȼ��ʱ��
    private float ignitedTimer;
    //������ʱ��
    private float chilledTimer;
    //�𾪶�ʱ��
    private float shockedTimer;

    //��ȼ�˺���ȴ
    private float igniteDamageCoodlown = 0.3f;
    //��ȼ�˺���ʱ��
    private float igniteDamageTimer;
    //��ȼ�˺�
    private int igniteDamage;
    //���Ԥ����
    [SerializeField] private GameObject shockStrikePrefab;
    //����˺�
    private int shockDamage;

    //��ǰ��Ѫ��
    public int currentHealth;

    //Unity��ʹ��Action���м򵥵�ί���¼�����
    //������Unity���¼���������ʹ��ʱҪ��֤�������������͡�˳�������ȫһ��
    //�ڽ���ֵ�ı��ʱ��
    public System.Action onHealthChanged;
    //�Ƿ�����
    public bool isDead { get; private set; }
    //�Ƿ����
    private bool isVulnerable;

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();

        entityFX = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
        {
            isIgnited = false;
        }

        if (chilledTimer < 0)
        {
            isChilled = false;
        }

        if (shockedTimer < 0)
        {
            isShocked = false;
        }

        //��ȼ�˺�
        if (isIgnited)
        {
            ApplyIgniteDamage();
        }
    }

    /// <summary>
    /// ʹ�������˺�
    /// </summary>
    /// <param name="_duration"></param>
    public void MakeVulnerableFor(float _duration)
    {
        StartCoroutine(VulnerableForCoroutine(_duration));
    }

    /// <summary>
    /// �������ܹ�����Э��
    /// </summary>
    /// <param name="_duaartion"></param>
    /// <returns></returns>
    private IEnumerator VulnerableForCoroutine(float _duaartion)
    {
        isVulnerable = true;

        yield return new WaitForSeconds(_duaartion);

        isVulnerable = false;
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="_modifier">���ӵľ�����ֵ</param>
    /// <param name="_duration">����ʱ��</param>
    /// <param name="_statToModify">Ҫ�޸ĵ�����</param>
    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statToModify)
    {
        //�����޸�Э�̡�
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));
    }

    /// <summary>
    /// �����޸�Э��
    /// </summary>
    /// <param name="_modifier">���ӵľ�����ֵ</param>
    /// <param name="_duration">����ʱ��</param>
    /// <param name="_statToModify">Ҫ�޸ĵ�����</param>
    /// <returns></returns>
    private IEnumerator StatModCoroutine(int _modifier, float _duration, Stat _statToModify)
    {
        _statToModify.AddModifier(_modifier);

        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);
    }

    /// <summary>
    /// ��������˺�
    /// </summary>
    /// <param name="_targetStats"></param>
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        //����
        if (TargetCanAvoidAttack(_targetStats))
        {
            return;
        }
        //�˺�
        int totalDamage = damage.GetValue() + strength.GetValue();
        //����
        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }
        //����
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);

        DoMagicalDamage(_targetStats);
    }

    //ħ���˺���Ԫ��
    #region Magical damage and elements
    /// <summary>
    /// ���ħ���˺�
    /// </summary>
    /// <param name="_targetStats"></param>
    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        //�����˺�
        int _fireDamage = fireDamage.GetValue();
        //��˪�˺�
        int _iceDamage = iceDamage.GetValue();
        //�׵��˺�
        int _lightningDamage = lightningDamage.GetValue();
        //���˺�
        int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        //ħ��
        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);

        _targetStats.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
        {
            return;
        }

        AttemptToApplyElements(_targetStats, _fireDamage, _iceDamage, _lightningDamage);
    }

    /// <summary>
    /// ����Ӧ��Ԫ��
    /// </summary>
    /// <param name="_targetStats">ħ��</param>
    /// <param name="_fireDamage">�����˺�</param>
    /// <param name="_iceDamage">��˪�˺�</param>
    /// <param name="_lightningDamage">�׵��˺�</param>
    private void AttemptToApplyElements(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightningDamage)
    {
        //���Ե�ȼ
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        //���Ա���
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        //������
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.Range(0, 3) == 0 && _fireDamage > 0)
            {
                canApplyIgnite = true;
                break;
            }
            else if (Random.Range(0, 3) == 1 && _iceDamage > 0)
            {
                canApplyChill = true;
                break;
            }
            if (Random.Range(0, 3) == 2 && _lightningDamage > 0)
            {
                canApplyShock = true;
                break;
            }
        }

        if (canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * 0.2f));
        }

        if (canApplyShock)
        {
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightningDamage * 0.1f));
        }

        //Ӧ��Ԫ��
        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    /// <summary>
    /// Ӧ�����е�Ԫ��
    /// </summary>
    /// <param name="_ignite">��ȼ</param>
    /// <param name="_chill">����</param>
    /// <param name="_shock">��</param>
    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = elementsDuartion;

            entityFX.IgniteFxFor(elementsDuartion);
        }

        if (_chill && canApplyChill)
        {
            isChilled = _chill;
            chilledTimer = elementsDuartion;

            float slowPercentage = 0.2f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, elementsDuartion);
            entityFX.ChillFxFor(elementsDuartion);
        }

        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                //ֻ���ڵ������������Ŀ��
                if (GetComponent<Player>() != null)
                {
                    return;
                }

                HitNearestTargetWithShockStrike();
            }
        }
    }

    /// <summary>
    /// Ӧ�ó��
    /// </summary>
    /// <param name="_shock"></param>
    public void ApplyShock(bool _shock)
    {
        if (isShocked)
        {
            return;
        }

        isShocked = _shock;
        shockedTimer = elementsDuartion;

        entityFX.ShorckFxFor(elementsDuartion);
    }

    /// <summary>
    /// �ó�����������Ŀ��
    /// </summary>
    private void HitNearestTargetWithShockStrike()
    {
        //��ȡλ��Բ�������ڵ�����2D��Ϸ��ײ����б�  
        //��һ������ point	Բ�ε����ġ�
        //�ڶ������� radius	Բ�εİ뾶��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        //�������
        float closestDistance = Mathf.Infinity;
        //����Ĺ���
        Transform closestEnemy = null;

        //������ײ����������Ƿ���ڵ��ˣ�������ڣ�����õ�������е�Damage����
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                //����A��B֮��ľ���
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)
            {
                closestEnemy = transform;
            }
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab,
                transform.position, Quaternion.identity);

            newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockDamage,
                closestEnemy.GetComponent<CharacterStats>());
        }
    }

    /// <summary>
    /// ��ȼ�˺�
    /// </summary>
    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0)
        {
            DecreaseHealthBy(igniteDamage);

            if (currentHealth < 0 && !isDead)
            {
                Die();
            }

            igniteDamageTimer = igniteDamageCoodlown;
        }
    }

    /// <summary>
    /// ���õ�ȼ�˺�
    /// </summary>
    public void SetupIgniteDamage(int _damage)
    {
        igniteDamage = _damage;
    }

    /// <summary>
    /// ���ó���˺�
    /// </summary>
    public void SetupShockStrikeDamage(int _damage)
    {
        shockDamage = _damage;
    }
    #endregion

    /// <summary>
    /// �ܵ��˺�
    /// </summary>
    /// <param name="_damage"></param>
    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact();
        entityFX.StartCoroutine("FlashFX");

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    /// <summary>
    /// ���ӽ���ֵ
    /// </summary>
    /// <param name="_amount"></param>
    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;

        if (currentHealth > GetMaxHealthValue())
        {
            currentHealth = GetMaxHealthValue();
        }

        if (onHealthChanged != null)
        {
            onHealthChanged();
        }
    }

    /// <summary>
    /// ���ٽ���ֵ
    /// </summary>
    /// <param name="_damage"></param>
    protected virtual void DecreaseHealthBy(int _damage)
    {
        if (isVulnerable)
        {
            _damage = Mathf.RoundToInt(_damage * 1.25f);
        }

        currentHealth -= _damage;

        if (onHealthChanged != null)
        {
            onHealthChanged();
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    protected virtual void Die()
    {
        isDead = true;
    }

    /// <summary>
    /// ���ʱ
    /// </summary>
    public virtual void OnEvasion()
    {

    }

    //ͳ�Ƽ���
    #region Stat calculations
    /// <summary>
    /// Ŀ���Ƿ���Ա��⹥��
    /// </summary>
    /// <param name="_targetStats"></param>
    /// <returns></returns>
    protected bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        //�𾪽���������20%
        if (isShocked)
        {
            totalEvasion -= 20;

        }

        if (totalEvasion < 0)
        {
            totalEvasion = 0;
        }

        if (Random.Range(0, 100) < totalEvasion)
        {
            _targetStats.OnEvasion();
            return true;
        }

        return false;
    }

    /// <summary>
    /// ���Ŀ�껤��
    /// </summary>
    /// <param name="_targetStats"></param>
    /// <param name="totalDamage"></param>
    /// <returns></returns>
    protected int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
        {
            //��������20%�Ļ���
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f);
        }
        else
        {
            totalDamage -= _targetStats.armor.GetValue();
        }

        //��������ĸ���ֵС����Сֵ���򷵻���Сֵ���������ֵ�������ֵ���򷵻����ֵ
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    /// <summary>
    /// ���Ŀ��ֿ���
    /// </summary>
    /// <param name="_targetStats"></param>
    /// <param name="totalMagicalDamage"></param>
    /// <returns></returns>
    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        //��������ĸ���ֵС����Сֵ���򷵻���Сֵ���������ֵ�������ֵ���򷵻����ֵ
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    /// <summary>
    /// �Ƿ񱩻�
    /// </summary>
    /// <returns></returns>
    protected bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// �����˺�
    /// </summary>
    /// <param name="_damage"></param>
    /// <returns></returns>
    protected int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        float critDamage = _damage * totalCritPower;
        //�����������������
        return Mathf.RoundToInt(critDamage);
    }

    /// <summary>
    /// ��ȡ��󽡿�ֵ
    /// </summary>
    /// <returns></returns>
    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
    #endregion

    /// <summary>
    /// ��ȡͳ��
    /// </summary>
    /// <returns></returns>
    public Stat GetStat(StatType _statType)
    {
        if (_statType == StatType.strength)
        {
            return strength;
        }
        else if (_statType == StatType.agility)
        {
            return agility;
        }
        else if (_statType == StatType.intelligence)
        {
            return intelligence;
        }
        else if (_statType == StatType.vitality)
        {
            return vitality;
        }
        else if (_statType == StatType.maxHealth)
        {
            return maxHealth;
        }
        else if (_statType == StatType.armor)
        {
            return armor;
        }
        else if (_statType == StatType.evasion)
        {
            return evasion;
        }
        else if (_statType == StatType.magicResistance)
        {
            return magicResistance;
        }
        else if (_statType == StatType.damage)
        {
            return damage;
        }
        else if (_statType == StatType.critChance)
        {
            return critChance;
        }
        else if (_statType == StatType.critPower)
        {
            return critPower;
        }
        else if (_statType == StatType.fireDamage)
        {
            return fireDamage;
        }
        else if (_statType == StatType.iceDamage)
        {
            return iceDamage;
        }
        else if (_statType == StatType.lightningDamage)
        {
            return lightningDamage;
        }

        return null;
    }
}
