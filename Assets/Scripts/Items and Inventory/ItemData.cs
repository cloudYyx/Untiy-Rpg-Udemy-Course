using System.Text;
using UnityEngine;

/// <summary>
/// 物品类型
/// </summary>
public enum ItemType
{
    //材料
    Material,
    //装备
    Equipment
}

// 在 Assets 下添加菜单
// fileName 生成名为 New Item Data的脚本
// menuName 菜单按钮名Data/Item
// order    按钮显示顺序
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
/// <summary>
/// 物品数据
/// ScriptableObject : 如果要创建独立于游戏对象的对象，则可以从中派生的类。
/// </summary>
public class ItemData : ScriptableObject
{
    //物品类型
    public ItemType itemType;
    //物品名称
    public string itemName;
    //Sprite : 表示在 2D 游戏中使用的精灵对象
    //图标
    public Sprite icon;

    [Range(0,100)]
    //掉落机会
    public float dropChance;

    protected StringBuilder sb = new StringBuilder();

    /// <summary>
    /// 获取描述
    /// </summary>
    /// <returns></returns>
    public virtual string GetDescription()
    {
        return "";
    }
}
