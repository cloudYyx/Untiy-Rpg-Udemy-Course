using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ������-UI
/// </summary>
public class UI_HealthBar : MonoBehaviour
{
    //ʵ��
    private Entity entity;
    //��������ͳ��
    private CharacterStats characterState;
    //���ε�λ�á���С��ê���������Ϣ��
    private RectTransform rectTransform;
    //��׼��������������С�����ֵ֮���ƶ���
    private Slider slider;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        characterState = GetComponentInParent<CharacterStats>();
        slider = GetComponentInChildren<Slider>();

        //ÿ�ε���entity.onFlippeed��ʱ�����ӵ���FlipUI
        entity.onFlippeed += FlipUI;
        //ÿ�ε���characterState.onHealthChanged��ʱ�����ӵ���UpdateHealthUI
        characterState.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }

    /// <summary>
    /// ����UI�Ľ���ֵ
    /// </summary>
    private void UpdateHealthUI()
    {
        slider.maxValue = characterState.GetMaxHealthValue();
        slider.value = characterState.currentHealth;
    }

    /// <summary>
    /// ��תUI
    /// </summary>
    private void FlipUI()
    {
        rectTransform.Rotate(0, 180, 0);
    }

    /// <summary>
    /// �����屻ȡ�������ʱ��ִ�еĴ��� 
    /// </summary>
    private void OnDisable()
    {
        entity.onFlippeed -= FlipUI;
        characterState.onHealthChanged -= UpdateHealthUI;
    }
}
