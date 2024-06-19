using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// UI统计槽
/// IPointerEnterHandler : 响应鼠标进入自身碰撞体范围事件
/// IPointerExitHandler ： 响应鼠标离开自身碰撞体范围事件
/// </summary>
public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //用户页面
    private UI ui;

    //统计名字
    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    //统计值文本
    [SerializeField] private TextMeshProUGUI statValueText;
    //统计名称文本
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    //统计描述
    [SerializeField] private string statDescription;

    /// <summary>
    /// Unity 在加载脚本或检查器中的值更改时调用的仅限编辑器的函数。
    /// </summary>
    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;

        if (statNameText != null)
        {
            statNameText.text = statName;
        }
    }

    void Start()
    {
        UpdateStatValueUI();

        ui = GetComponentInParent<UI>();
    }

    /// <summary>
    /// 更新统计值UI
    /// </summary>
    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            statValueText.text = playerStats.GetStat(statType).GetValue().ToString();


            if (statType == StatType.maxHealth)
            {
                statValueText.text = playerStats.GetMaxHealthValue().ToString();
            }

            if (statType == StatType.damage)
            {
                statValueText.text = (playerStats.damage.GetValue() + playerStats.strength.GetValue()).ToString();
            }

            if (statType == StatType.critPower)
            {
                statValueText.text = (playerStats.critPower.GetValue() + playerStats.strength.GetValue()).ToString();
            }

            if (statType == StatType.critChance)
            {
                statValueText.text = (playerStats.critChance.GetValue() + playerStats.agility.GetValue()).ToString();
            }

            if (statType == StatType.evasion)
            {
                statValueText.text = (playerStats.evasion.GetValue() + playerStats.agility.GetValue()).ToString();
            }

            if (statType == StatType.magicResistance)
            {
                statValueText.text = (playerStats.magicResistance.GetValue() + (playerStats.intelligence.GetValue() * 3)).ToString();
            }
        }
    }

    /// <summary>
    /// 响应鼠标进入自身碰撞体范围事件
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statToolTip.ShowStatToolTip(statDescription);
    }

    /// <summary>
    /// 响应鼠标离开自身碰撞体范围事件
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statToolTip.HideStatToolTip();
    }
}
