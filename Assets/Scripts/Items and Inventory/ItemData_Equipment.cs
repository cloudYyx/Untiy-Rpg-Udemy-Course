using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// װ������
/// </summary>
public enum EquipmentType
{
    //����
    Weapon,
    //����
    Armor,
    //�����
    Amulet,
    //ƿ
    Flask
}

// �� Assets ����Ӳ˵�
// fileName ������Ϊ New Item Data�Ľű�
// menuName �˵���ť��Data/Item
// order    ��ť��ʾ˳��
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
/// <summary>
/// ��Ʒ����װ������
/// </summary>
public class ItemData_Equipment : ItemData
{
    //װ������
    public EquipmentType equipmentType;

    [Header("Unique effect")]
    //��Ʒ��ȴ
    public float itemCooldown;
    //��ƷЧ��
    public ItemEffect[] itemEffects;
    [TextArea]
    //��ƷЧ������
    public string itemEffectDescription;

    //��Ҫ����
    [Header("Major stats")]
    //���� 1������1���˺���1%��������
    public int strength;
    //���� 1������1%�����ʺ�1%��������
    public int agility;
    //���� 1������1��ħ���˺���3��ħ������
    public int intelligence;
    //���� 1������5������ֵ
    public int vitality;

    //��������
    [Header("Defensive stats")]
    //���Ѫ��
    public int maxHealth;
    //����
    public int armor;
    //������
    public int evasion;
    //ħ������
    public int magicResistance;

    //��������
    [Header("Offensive stats")]
    //�˺�
    public int damage;
    //������
    public int critChance;
    //�������� Ĭ�� 150%
    public int critPower;

    //ħ������
    [Header("Magic stats")]
    //�����˺�
    public int fireDamage;
    //��˪�˺�
    public int iceDamage;
    //�׵��˺�
    public int lightningDamage;

    //����Ҫ��
    [Header("Craft requirements")]
    //�ֹ���������
    public List<InventoryItem> craftingMaterials;

    //��������
    private int descriptionLength;

    /// <summary>
    /// ִ����ƷЧ��
    /// </summary>
    public void Effect(Transform _enemyPosition)
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }

    /// <summary>
    /// ������δ�
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
    /// ɾ�����δ�
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
    /// ��ȡ����
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
    /// �����Ʒ����
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
