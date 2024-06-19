using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 在 Assets 下添加菜单
// fileName 生成名为 Thunder strike effect的脚本
// menuName 菜单按钮名Data/Item effect/Thunder strike
// order    按钮显示顺序
[CreateAssetMenu(fileName = "Thunder strike effect", menuName = "Data/Item effect/Thunder strike")]
/// <summary>
/// 雷击效果
/// </summary>
public class ThunderStrike_Effect : ItemEffect
{
    //雷击预制体
    [SerializeField] private GameObject thunderStrikePrefab;

    /// <summary>
    /// 执行的效果
    /// </summary>
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        //Object 实例化的克隆对象。
        //swordPrefab是要复制的现有对象。
        //position	新对象的位置。
        //rotation 新对象的方向。
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, _enemyPosition.position, Quaternion.identity);

        Destroy(newThunderStrike,1f);
    }
}
