using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 生命条-UI
/// </summary>
public class UI_HealthBar : MonoBehaviour
{
    //实体
    private Entity entity;
    //人物属性统计
    private CharacterStats characterState;
    //矩形的位置、大小、锚点和轴心信息。
    private RectTransform rectTransform;
    //标准滑动条，可在最小和最大值之间移动。
    private Slider slider;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        characterState = GetComponentInParent<CharacterStats>();
        slider = GetComponentInChildren<Slider>();

        //每次调用entity.onFlippeed的时候会添加调用FlipUI
        entity.onFlippeed += FlipUI;
        //每次调用characterState.onHealthChanged的时候会添加调用UpdateHealthUI
        characterState.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }

    /// <summary>
    /// 更新UI的健康值
    /// </summary>
    private void UpdateHealthUI()
    {
        slider.maxValue = characterState.GetMaxHealthValue();
        slider.value = characterState.currentHealth;
    }

    /// <summary>
    /// 翻转UI
    /// </summary>
    private void FlipUI()
    {
        rectTransform.Rotate(0, 180, 0);
    }

    /// <summary>
    /// 当物体被取消激活的时候执行的代码 
    /// </summary>
    private void OnDisable()
    {
        entity.onFlippeed -= FlipUI;
        characterState.onHealthChanged -= UpdateHealthUI;
    }
}
