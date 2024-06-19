using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// UIͳ�Ʋ�
/// IPointerEnterHandler : ��Ӧ������������ײ�巶Χ�¼�
/// IPointerExitHandler �� ��Ӧ����뿪������ײ�巶Χ�¼�
/// </summary>
public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //�û�ҳ��
    private UI ui;

    //ͳ������
    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    //ͳ��ֵ�ı�
    [SerializeField] private TextMeshProUGUI statValueText;
    //ͳ�������ı�
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    //ͳ������
    [SerializeField] private string statDescription;

    /// <summary>
    /// Unity �ڼ��ؽű��������е�ֵ����ʱ���õĽ��ޱ༭���ĺ�����
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
    /// ����ͳ��ֵUI
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
    /// ��Ӧ������������ײ�巶Χ�¼�
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statToolTip.ShowStatToolTip(statDescription);
    }

    /// <summary>
    /// ��Ӧ����뿪������ײ�巶Χ�¼�
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statToolTip.HideStatToolTip();
    }
}
