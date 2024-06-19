using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �û�����
/// </summary>
public class UI : MonoBehaviour
{
    //�ַ�����
    [SerializeField] private GameObject characterUI;
    //���ܽ���
    [SerializeField] private GameObject skillUI;
    //���ս���
    [SerializeField] private GameObject craftUI;
    //���ý���
    [SerializeField] private GameObject optionsUI;

    //��Ʒ������ʾUI
    public UI_itemTooltip itemTooltip;
    //ͳ�ƹ�����ʾUI
    public UI_StatToolTip statToolTip;
    //���մ���UI
    public UI_CraftWindow craftWindow;
    //���ܹ�����ʾUI
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
    /// �л���Ϸ����
    /// </summary>
    /// <param name="_menu"></param>
    public void SwitchTo(GameObject _menu)
    {
        //���˴���������Ϸ�������඼������
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        //������Ϸ����
        if (_menu != null)
        {
            _menu.SetActive(true);
        }
    }

    /// <summary>
    /// �ùؼ����л�����Ϸ����
    /// </summary>
    /// <param name="_menu"></param>
    public void SwitchWithKeyTo(GameObject _menu)
    {
        //activeSelf �� GameObject �ı��ػ״̬����ֻ����
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            return;
        }

        SwitchTo(_menu);
    }
}
