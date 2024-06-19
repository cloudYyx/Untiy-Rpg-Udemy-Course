using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敌人属性统计
/// </summary>
public class EnemyStats : CharacterStats
{
    //敌人
    private Enemy enemy;
    //掉落物品
    private ItemDrop myDropSystem;

    [Header("Level details")]
    //等级
    [SerializeField] private int level;

    [Range(0f, 1f)]
    //百分比修饰符
    [SerializeField] private float percentageModifier;


    protected override void Start()
    {
        ApplyLevelModifiers();

        base.Start();

        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();
    }

    /// <summary>
    /// 应用等级修改器
    /// </summary>
    private void ApplyLevelModifiers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightningDamage);
    }

    /// <summary>
    /// 属性修改器
    /// </summary>
    /// <param name="_stat"></param>
    private void Modify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * percentageModifier;

            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();

        enemy.Die();

        myDropSystem.GenerateDrop();
    }
}
