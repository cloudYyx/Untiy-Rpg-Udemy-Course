using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��ƷЧ��
/// ScriptableObject : ���Ҫ������������Ϸ����Ķ�������Դ����������ࡣ
/// </summary>
public class ItemEffect : ScriptableObject
{
    /// <summary>
    /// ִ�е�Ч��
    /// </summary>
    public virtual void ExecuteEffect(Transform _enemyPosition)
    {
        Debug.Log(1);
    }
}
