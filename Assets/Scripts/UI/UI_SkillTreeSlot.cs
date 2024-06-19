using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// ��������UI
/// IPointerEnterHandler : ��Ӧ������������ײ�巶Χ�¼�
/// IPointerExitHandler �� ��Ӧ����뿪������ײ�巶Χ�¼�
/// </summary>
public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //�û�ҳ��
    private UI ui;
    //����ͼ��
    private Image skillImage;

    //���ܵļ۸�
    [SerializeField] private int skillPrice;
    //��������
    [SerializeField] private string skillName;
    [TextArea]
    //��������
    [SerializeField] private string skillDescription;
    //����������ɫ
    [SerializeField] private Color lockedSkillColor;

    //�Ƿ����
    public bool unlocked;

    //Ӧ���ǽ�����
    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    //Ӧ�������ŵ�
    [SerializeField] private UI_SkillTreeSlot[] shouldBelocked;


    /// <summary>
    /// Unity �ڼ��ؽű��������е�ֵ����ʱ���õĽ��ޱ༭���ĺ���
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
    /// �������ܲ�
    /// </summary>
    public void UnlockSkillSlot()
    {
        //ǰ�ü���û�����޷�����
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked == false)
            {
                Debug.Log("�޷���������");
                return;
            }
        }

        //��ͻ���ܽ����޷������ü���
        for (int i = 0; i < shouldBelocked.Length; i++)
        {
            if (shouldBelocked[i].unlocked == true)
            {
                Debug.Log("�޷���������");
                return;
            }
        }

        //û���㹻��Ǯ��ʧ��
        if (PlayerManager.instance.HaveEnoughMoney(skillPrice) == false)
        {
            return;
        }

        unlocked = true;
        skillImage.color = Color.white;
    }


    /// <summary>
    /// ��Ӧ������������ײ�巶Χ�¼�
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillDescription,skillName);

        //�������λ����ʾ��Ӧ�ļ�������
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
    /// ��Ӧ����뿪������ײ�巶Χ�¼�
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }
}
