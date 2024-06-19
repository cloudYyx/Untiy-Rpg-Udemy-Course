using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能管理器
/// </summary>
public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public Dash_Skill dash { get; private set; }
    public Clone_Skill clone { get; private set; }
    public Sword_Skill sword { get; private set; }
    public Blackhole_Skill blackhole { get; private set; }
    public Crystal_Skill crystal { get; private set; }
    public Parry_Skill parry { get; private set; }
    public Dodge_Skill dodge { get; private set; }

    private void Awake()
    {
        //单例模式
        if (instance != null)
        {
            //移除一个游戏对象
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        dash = GetComponent<Dash_Skill>();
        clone = GetComponent<Clone_Skill>();
        sword = GetComponent<Sword_Skill>();
        blackhole = GetComponent<Blackhole_Skill>();
        crystal = GetComponent<Crystal_Skill>();
        parry = GetComponent<Parry_Skill>();
        dodge = GetComponent<Dodge_Skill>();
    }
}
