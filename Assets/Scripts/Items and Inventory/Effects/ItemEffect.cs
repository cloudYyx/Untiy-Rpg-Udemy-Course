using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 物品效果
/// ScriptableObject : 如果要创建独立于游戏对象的对象，则可以从中派生的类。
/// </summary>
public class ItemEffect : ScriptableObject
{
    /// <summary>
    /// 执行的效果
    /// </summary>
    public virtual void ExecuteEffect(Transform _enemyPosition)
    {
        Debug.Log(1);
    }
}
