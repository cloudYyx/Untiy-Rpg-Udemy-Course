using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// �� Assets ����Ӳ˵�
// fileName ������Ϊ Heal effect�Ľű�
// menuName �˵���ť��Data/Item effect/Heal effect
// order    ��ť��ʾ˳��
[CreateAssetMenu(fileName = "Heal effect", menuName = "Data/Item effect/Heal effect")]
/// <summary>
/// ����Ч��
/// </summary>
public class Heal_Effect : ItemEffect
{
    [Range(0f, 1f)]
    //�����ٷֱ�
    [SerializeField] private float healPercent;

    /// <summary>
    /// ִ�е�Ч��
    /// </summary>
    /// <param name="_enemyPosition"></param>
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        //������
        int healAmount = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPercent);

        playerStats.IncreaseHealthBy(healAmount);        
    }
}
