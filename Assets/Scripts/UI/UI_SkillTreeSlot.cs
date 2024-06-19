using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 技能树槽UI
/// IPointerEnterHandler : 响应鼠标进入自身碰撞体范围事件
/// IPointerExitHandler ： 响应鼠标离开自身碰撞体范围事件
/// </summary>
public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //用户页面
    private UI ui;
    //技能图标
    private Image skillImage;

    //技能的价格
    [SerializeField] private int skillPrice;
    //技能名字
    [SerializeField] private string skillName;
    [TextArea]
    //技能描述
    [SerializeField] private string skillDescription;
    //锁定技能颜色
    [SerializeField] private Color lockedSkillColor;

    //是否解锁
    public bool unlocked;

    //应该是解锁的
    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    //应该是锁着的
    [SerializeField] private UI_SkillTreeSlot[] shouldBelocked;


    /// <summary>
    /// Unity 在加载脚本或检查器中的值更改时调用的仅限编辑器的函数
    /// </summary>
    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillName;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());

    }

    private void Start()
    {
        skillImage = GetComponent<Image>();
        ui = GetComponentInParent<UI>();

        skillImage.color = lockedSkillColor;
    }

    /// <summary>
    /// 解锁技能槽
    /// </summary>
    public void UnlockSkillSlot()
    {
        //前置技能没点亮无法解锁
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked == false)
            {
                Debug.Log("无法解锁技能");
                return;
            }
        }

        //冲突技能解锁无法解锁该技能
        for (int i = 0; i < shouldBelocked.Length; i++)
        {
            if (shouldBelocked[i].unlocked == true)
            {
                Debug.Log("无法解锁技能");
                return;
            }
        }

        //没有足够的钱则失败
        if (PlayerManager.instance.HaveEnoughMoney(skillPrice) == false)
        {
            return;
        }

        unlocked = true;
        skillImage.color = Color.white;
    }


    /// <summary>
    /// 响应鼠标进入自身碰撞体范围事件
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillDescription,skillName);

        //根据鼠标位置显示相应的技能描述
        Vector2 mousePosition = Input.mousePosition;

        float xOffset = 0;
        float yOffset = 0;

        if (mousePosition.x > 200)
        {
            xOffset = -150;
        }
        else
        {
            xOffset = 150;
        }

        if (mousePosition.y > 200)
        {
            yOffset = -150;
        }
        else
        {
            yOffset = 150;
        }

        ui.skillToolTip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }

    /// <summary>
    /// 响应鼠标离开自身碰撞体范围事件
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }
}
