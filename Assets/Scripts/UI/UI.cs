using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 用户界面
/// </summary>
public class UI : MonoBehaviour
{
    //字符界面
    [SerializeField] private GameObject characterUI;
    //技能界面
    [SerializeField] private GameObject skillUI;
    //工艺界面
    [SerializeField] private GameObject craftUI;
    //设置界面
    [SerializeField] private GameObject optionsUI;

    //物品工具提示UI
    public UI_itemTooltip itemTooltip;
    //统计工具提示UI
    public UI_StatToolTip statToolTip;
    //工艺窗口UI
    public UI_CraftWindow craftWindow;
    //技能工具提示UI
    public UI_SkillToolTip skillToolTip;

    private void Awake()
    {
        SwitchTo(skillUI);
    }

    void Start()
    {
        SwitchTo(null);

        itemTooltip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchWithKeyTo(characterUI);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchWithKeyTo(craftUI);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchWithKeyTo(skillUI);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SwitchWithKeyTo(optionsUI);
        }
    }

    /// <summary>
    /// 切换游戏对象
    /// </summary>
    /// <param name="_menu"></param>
    public void SwitchTo(GameObject _menu)
    {
        //除了传进来的游戏对象，其余都不激活
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        //激活游戏对象
        if (_menu != null)
        {
            _menu.SetActive(true);
        }
    }

    /// <summary>
    /// 用关键字切换到游戏对象
    /// </summary>
    /// <param name="_menu"></param>
    public void SwitchWithKeyTo(GameObject _menu)
    {
        //activeSelf 此 GameObject 的本地活动状态。（只读）
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            return;
        }

        SwitchTo(_menu);
    }
}
