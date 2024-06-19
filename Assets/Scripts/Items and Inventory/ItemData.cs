using System.Text;
using UnityEngine;

/// <summary>
/// ��Ʒ����
/// </summary>
public enum ItemType
{
    //����
    Material,
    //װ��
    Equipment
}

// �� Assets ����Ӳ˵�
// fileName ������Ϊ New Item Data�Ľű�
// menuName �˵���ť��Data/Item
// order    ��ť��ʾ˳��
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
/// <summary>
/// ��Ʒ����
/// ScriptableObject : ���Ҫ������������Ϸ����Ķ�������Դ����������ࡣ
/// </summary>
public class ItemData : ScriptableObject
{
    //��Ʒ����
    public ItemType itemType;
    //��Ʒ����
    public string itemName;
    //Sprite : ��ʾ�� 2D ��Ϸ��ʹ�õľ������
    //ͼ��
    public Sprite icon;

    [Range(0,100)]
    //�������
    public float dropChance;

    protected StringBuilder sb = new StringBuilder();

    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <returns></returns>
    public virtual string GetDescription()
    {
        return "";
    }
}
