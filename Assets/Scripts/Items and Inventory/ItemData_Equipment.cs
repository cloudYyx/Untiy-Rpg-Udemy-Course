using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 装备类型
/// </summary>
public enum EquipmentType
{
    //武器
    Weapon,
    //护甲
    Armor,
    //护身符
    Amulet,
    //瓶
    Flask
}

// 在 Assets 下添加菜单
// fileName 生成名为 New Item Data的脚本
// menuName 菜单按钮名Data/Item
// order    按钮显示顺序
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
/// <summary>
/// 物品——装备数据
/// </summary>
public class ItemData_Equipment : ItemData
{
    //装备类型
    public EquipmentType equipmentType;

    [Header("Unique effect")]
    //物品冷却
    public float itemCooldown;
    //物品效果
    public ItemEffect[] itemEffects;
    [TextArea]
    //物品效果描述
    public string itemEffectDescription;

    //主要属性
    [Header("Major stats")]
    //力量 1点增加1点伤害和1%暴击威力
    public int strength;
    //敏捷 1点增加1%闪避率和1%暴击几率
    public int agility;
    //智力 1点增加1点魔法伤害和3点魔法抗性
    public int intelligence;
    //体力 1点增加5点生命值
    public int vitality;

    //防御属性
    [Header("Defensive stats")]
    //最大血量
    public int maxHealth;
    //护甲
    public int armor;
    //闪避率
    public int evasion;
    //魔法抗性
    public int magicResistance;

    //攻击属性
    [Header("Offensive stats")]
    //伤害
    public int damage;
    //暴击率
    public int critChance;
    //暴击威力 默认 150%
    public int critPower;

    //魔法属性
    [Header("Magic stats")]
    //火焰伤害
    public int fireDamage;
    //冰霜伤害
    public int iceDamage;
    //雷电伤害
    public int lightningDamage;

    //工艺要求
    [Header("Craft requirements")]
    //手工制作材料
    public List<InventoryItem> craftingMaterials;

    //描述长度
    private int descriptionLength;

    /// <summary>
    /// 执行物品效果
    /// </summary>
    public void Effect(Transform _enemyPosition)
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }

    /// <summary>
    /// 添加修饰词
    /// </summary>
    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.maxHealth.AddModifier(maxHealth);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightningDamage.AddModifier(lightningDamage);
    }

    /// <summary>
    /// 删除修饰词
    /// </summary>
    public void RemoveModifiers() 
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.maxHealth.RemoveModifier(maxHealth);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightningDamage.RemoveModifier(lightningDamage);
    }

    /// <summary>
    /// 获取描述
    /// </summary>
    /// <returns></returns>
    public override string GetDescription()
    {
        sb.Length = 0;
        descriptionLength = 0;

        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(vitality, "Vitality");

        AddItemDescription(maxHealth, "maxHealth");
        AddItemDescription(armor, "Armor");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(magicResistance, "Resistance");

        AddItemDescription(damage, "Damage");
        AddItemDescription(critChance, "critChance");
        AddItemDescription(critPower, "critPower");

        AddItemDescription(fireDamage, "fire");
        AddItemDescription(iceDamage, "ice");
        AddItemDescription(lightningDamage, "lightning");

        if (descriptionLength < 5)
        {
            for(int i = 0; i < 5 - descriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }

        if (itemEffectDescription.Length > 0)
        {
            sb.AppendLine();
            sb.Append(itemEffectDescription);
        }

        return sb.ToString();
    }

    /// <summary>
    /// 添加物品描述
    /// </summary>
    /// <param name="_value"></param>
    /// <param name="_name"></param>
    private void AddItemDescription(int _value,string _name)
    {
        if (_value != 0)
        {
            if (sb.Length > 0)
            {
                sb.AppendLine();
            }

            if (_value > 0)
            {
                sb.Append("+ " + _value + " " + _name);
            }

            descriptionLength++;
        }
    }
}
