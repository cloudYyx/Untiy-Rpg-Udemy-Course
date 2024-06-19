using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// �� Assets ����Ӳ˵�
// fileName ������Ϊ Buff effect�Ľű�
// menuName �˵���ť��Data/Item effect/Buff effect
// order    ��ť��ʾ˳��
[CreateAssetMenu(fileName = "Buff effect", menuName = "Data/Item effect/Buff effect")]
/// <summary>
/// ����Ч��
/// </summary>
public class Buff_Effect : ItemEffect
{
    //�������ͳ��
    private PlayerStats stats;
    //��������
    [SerializeField] private StatType buffType;
    //��������
    [SerializeField] private int buffAmount;
    //�������ʱ��
    [SerializeField] private float buffDuration;

    /// <summary>
    /// ִ�е�Ч��
    /// </summary>
    /// <param name="_enemyPosition"></param>
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        stats.IncreaseStatBy(buffAmount, buffDuration, stats.GetStat(buffType));
    }
}
