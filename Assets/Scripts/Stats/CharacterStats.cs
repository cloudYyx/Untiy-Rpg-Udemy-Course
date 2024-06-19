
using System.Collections;
using UnityEngine;

/// <summary>
/// 增益类型
/// </summary>
public enum StatType
{
    strength,//力量
    agility,//敏捷
    intelligence,//智力
    vitality,//体力

    maxHealth,//最大血量
    armor,//护甲
    evasion,//闪避率
    magicResistance,//魔法抗性

    damage,//伤害
    critChance,//暴击率
    critPower,//暴击威力

    fireDamage,//火焰伤害
    iceDamage,//冰霜伤害
    lightningDamage//雷电伤害
}

/// <summary>
/// 人物属性
/// </summary>
public class CharacterStats : MonoBehaviour
{
    //闪光效果
    private EntityFX entityFX;

    //主要属性
    [Header("Major stats")]
    //力量 1点增加1点伤害和1%暴击威力
    public Stat strength;
    //敏捷 1点增加1%闪避率和1%暴击几率
    public Stat agility;
    //智力 1点增加1点魔法伤害和3点魔法抗性
    public Stat intelligence;
    //体力 1点增加5点生命值
    public Stat vitality;

    //防御属性
    [Header("Defensive stats")]
    //最大血量
    public Stat maxHealth;
    //护甲
    public Stat armor;
    //闪避率
    public Stat evasion;
    //魔法抗性
    public Stat magicResistance;

    //攻击属性
    [Header("Offensive stats")]
    //伤害
    public Stat damage;
    //暴击率
    public Stat critChance;
    //暴击威力 默认 150%
    public Stat critPower;

    //魔法属性
    [Header("Magic stats")]
    //火焰伤害
    public Stat fireDamage;
    //冰霜伤害
    public Stat iceDamage;
    //雷电伤害
    public Stat lightningDamage;

    //是否被点燃 伤害随着时间的推移
    public bool isIgnited;
    //是否被冰冻 减少20%的护甲
    public bool isChilled;
    //是否被震惊 降低准确率20%
    public bool isShocked;

    //元素持续时间
    [SerializeField] private float elementsDuartion = 4;
    //点燃定时器
    private float ignitedTimer;
    //冰冻定时器
    private float chilledTimer;
    //震惊定时器
    private float shockedTimer;

    //点燃伤害冷却
    private float igniteDamageCoodlown = 0.3f;
    //点燃伤害定时器
    private float igniteDamageTimer;
    //点燃伤害
    private int igniteDamage;
    //冲击预制体
    [SerializeField] private GameObject shockStrikePrefab;
    //冲击伤害
    private int shockDamage;

    //当前的血量
    public int currentHealth;

    //Unity中使用Action进行简单的委托事件管理
    //可以用Unity的事件处理，但是使用时要保证参数个数、类型、顺序必须完全一致
    //在健康值改变的时候
    public System.Action onHealthChanged;
    //是否死亡
    public bool isDead { get; private set; }
    //是否脆弱
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

        //点燃伤害
        if (isIgnited)
        {
            ApplyIgniteDamage();
        }
    }

    /// <summary>
    /// 使其易受伤害
    /// </summary>
    /// <param name="_duration"></param>
    public void MakeVulnerableFor(float _duration)
    {
        StartCoroutine(VulnerableForCoroutine(_duration));
    }

    /// <summary>
    /// 启动易受攻击的协程
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
    /// 增加属性
    /// </summary>
    /// <param name="_modifier">增加的具体数值</param>
    /// <param name="_duration">持续时间</param>
    /// <param name="_statToModify">要修改的数据</param>
    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statToModify)
    {
        //启动修改协程。
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));
    }

    /// <summary>
    /// 启动修改协程
    /// </summary>
    /// <param name="_modifier">增加的具体数值</param>
    /// <param name="_duration">持续时间</param>
    /// <param name="_statToModify">要修改的数据</param>
    /// <returns></returns>
    private IEnumerator StatModCoroutine(int _modifier, float _duration, Stat _statToModify)
    {
        _statToModify.AddModifier(_modifier);

        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);
    }

    /// <summary>
    /// 造成物理伤害
    /// </summary>
    /// <param name="_targetStats"></param>
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        //闪避
        if (TargetCanAvoidAttack(_targetStats))
        {
            return;
        }
        //伤害
        int totalDamage = damage.GetValue() + strength.GetValue();
        //暴击
        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }
        //防御
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);

        DoMagicalDamage(_targetStats);
    }

    //魔法伤害和元素
    #region Magical damage and elements
    /// <summary>
    /// 造成魔法伤害
    /// </summary>
    /// <param name="_targetStats"></param>
    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        //火焰伤害
        int _fireDamage = fireDamage.GetValue();
        //冰霜伤害
        int _iceDamage = iceDamage.GetValue();
        //雷电伤害
        int _lightningDamage = lightningDamage.GetValue();
        //总伤害
        int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        //魔抗
        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);

        _targetStats.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
        {
            return;
        }

        AttemptToApplyElements(_targetStats, _fireDamage, _iceDamage, _lightningDamage);
    }

    /// <summary>
    /// 尝试应用元素
    /// </summary>
    /// <param name="_targetStats">魔抗</param>
    /// <param name="_fireDamage">火焰伤害</param>
    /// <param name="_iceDamage">冰霜伤害</param>
    /// <param name="_lightningDamage">雷电伤害</param>
    private void AttemptToApplyElements(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightningDamage)
    {
        //可以点燃
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        //可以冰冻
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        //可以震惊
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

        //应用元素
        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    /// <summary>
    /// 应用所有的元素
    /// </summary>
    /// <param name="_ignite">点燃</param>
    /// <param name="_chill">冰冻</param>
    /// <param name="_shock">震惊</param>
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
                //只能在敌人中找最近的目标
                if (GetComponent<Player>() != null)
                {
                    return;
                }

                HitNearestTargetWithShockStrike();
            }
        }
    }

    /// <summary>
    /// 应用冲击
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
    /// 用冲击击中最近的目标
    /// </summary>
    private void HitNearestTargetWithShockStrike()
    {
        //获取位于圆形区域内的所有2D游戏碰撞体的列表  
        //第一参数是 point	圆形的中心。
        //第二参数是 radius	圆形的半径。
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        //最近距离
        float closestDistance = Mathf.Infinity;
        //最近的怪物
        Transform closestEnemy = null;

        //遍历碰撞器包，检测是否存在敌人，如果存在，则调用敌人组件中的Damage函数
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                //返回A到B之间的距离
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
    /// 点燃伤害
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
    /// 设置点燃伤害
    /// </summary>
    public void SetupIgniteDamage(int _damage)
    {
        igniteDamage = _damage;
    }

    /// <summary>
    /// 设置冲击伤害
    /// </summary>
    public void SetupShockStrikeDamage(int _damage)
    {
        shockDamage = _damage;
    }
    #endregion

    /// <summary>
    /// 受到伤害
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
    /// 增加健康值
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
    /// 减少健康值
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
    /// 死亡
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    protected virtual void Die()
    {
        isDead = true;
    }

    /// <summary>
    /// 躲避时
    /// </summary>
    public virtual void OnEvasion()
    {

    }

    //统计计算
    #region Stat calculations
    /// <summary>
    /// 目标是否可以避免攻击
    /// </summary>
    /// <param name="_targetStats"></param>
    /// <returns></returns>
    protected bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        //震惊降低闪避率20%
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
    /// 检查目标护甲
    /// </summary>
    /// <param name="_targetStats"></param>
    /// <param name="totalDamage"></param>
    /// <returns></returns>
    protected int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
        {
            //冰冻减少20%的护甲
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f);
        }
        else
        {
            totalDamage -= _targetStats.armor.GetValue();
        }

        //如果给定的浮点值小于最小值，则返回最小值。如果给定值大于最大值，则返回最大值
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    /// <summary>
    /// 检查目标抵抗力
    /// </summary>
    /// <param name="_targetStats"></param>
    /// <param name="totalMagicalDamage"></param>
    /// <returns></returns>
    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        //如果给定的浮点值小于最小值，则返回最小值。如果给定值大于最大值，则返回最大值
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    /// <summary>
    /// 是否暴击
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
    /// 暴击伤害
    /// </summary>
    /// <param name="_damage"></param>
    /// <returns></returns>
    protected int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        float critDamage = _damage * totalCritPower;
        //返回四舍五入的整数
        return Mathf.RoundToInt(critDamage);
    }

    /// <summary>
    /// 获取最大健康值
    /// </summary>
    /// <returns></returns>
    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
    #endregion

    /// <summary>
    /// 获取统计
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
