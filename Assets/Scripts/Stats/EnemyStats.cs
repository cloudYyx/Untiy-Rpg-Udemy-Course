using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��������ͳ��
/// </summary>
public class EnemyStats : CharacterStats
{
    //����
    private Enemy enemy;
    //������Ʒ
    private ItemDrop myDropSystem;

    [Header("Level details")]
    //�ȼ�
    [SerializeField] private int level;

    [Range(0f, 1f)]
    //�ٷֱ����η�
    [SerializeField] private float percentageModifier;


    protected override void Start()
    {
        ApplyLevelModifiers();

        base.Start();

        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();
    }

    /// <summary>
    /// Ӧ�õȼ��޸���
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
    /// �����޸���
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
