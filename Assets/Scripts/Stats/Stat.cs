using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ͳ��
/// </summary>
/// Serializable���Խ���public���͵����ݺͽṹ��ʾ��Inspector�����
[System.Serializable]
public class Stat
{
    //��׼ֵ
    [SerializeField] private int baseValue;
    //�޸���
    public List<int> modifiers;

    public int GetValue()
    {
        int finalValue = baseValue;

        foreach (int modifier in modifiers)
        {
            finalValue += modifier;
        }

        return finalValue;
    }

    /// <summary>
    /// ����Ĭ��ֵ
    /// </summary>
    /// <param name="_value"></param>
    public void SetDefaultValue(int _value)
    {
        baseValue = _value;
    }

    /// <summary>
    /// ������η�
    /// </summary>
    /// <param name="_modifier"></param>
    public void AddModifier(int _modifier)
    {
        modifiers.Add(_modifier);
    }

    /// <summary>
    /// ɾ�����η�
    /// </summary>
    /// <param name="_modifier"></param>
    public void RemoveModifier(int _modifier)
    {
        modifiers.Remove(_modifier);
    }
}
