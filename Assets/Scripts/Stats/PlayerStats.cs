using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ÕÊº“ Ù–‘Õ≥º∆
/// </summary>
public class PlayerStats : CharacterStats
{
    private Player player;

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();

        player.Die();
        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    /// <summary>
    /// ºı…ŸΩ°øµ÷µ
    /// </summary>
    /// <param name="_damage"></param>
    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);

        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);

        if (currentArmor != null) 
        {
            currentArmor.Effect(player.transform);
        }
    }

    /// <summary>
    /// …¡±‹ ±
    /// </summary>
    public override void OnEvasion()
    {
        player.skillManager.dodge.CreateMirageOnDoDodge();
    }

    /// <summary>
    /// øÀ¬°‘Ï≥……À∫¶
    /// </summary>
    public void CloneDoDamage(CharacterStats _targetStats,float _multiplier)
    {
        //…¡±‹
        if (TargetCanAvoidAttack(_targetStats))
        {
            return;
        }

        //…À∫¶
        int totalDamage = damage.GetValue() + strength.GetValue();

        if (_multiplier > 0)
        {
            totalDamage = Mathf.RoundToInt(totalDamage * _multiplier);
        }

        //±©ª˜
        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }
        //∑¿”˘
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);

        DoMagicalDamage(_targetStats);
    }
}
