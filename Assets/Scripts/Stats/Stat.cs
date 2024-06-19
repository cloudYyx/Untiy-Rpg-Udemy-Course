using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 统计
/// </summary>
/// Serializable可以将非public类型的数据和结构显示在Inspector面板上
[System.Serializable]
public class Stat
{
    //基准值
    [SerializeField] private int baseValue;
    //修改器
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
    /// 设置默认值
    /// </summary>
    /// <param name="_value"></param>
    public void SetDefaultValue(int _value)
    {
        baseValue = _value;
    }

    /// <summary>
    /// 添加修饰符
    /// </summary>
    /// <param name="_modifier"></param>
    public void AddModifier(int _modifier)
    {
        modifiers.Add(_modifier);
    }

    /// <summary>
    /// 删除修饰符
    /// </summary>
    /// <param name="_modifier"></param>
    public void RemoveModifier(int _modifier)
    {
        modifiers.Remove(_modifier);
    }
}
