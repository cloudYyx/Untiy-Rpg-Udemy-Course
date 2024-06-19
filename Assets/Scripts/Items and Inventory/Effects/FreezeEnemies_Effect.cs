using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� Assets ����Ӳ˵�
// fileName ������Ϊ Freeze enemeis effect�Ľű�
// menuName �˵���ť��Data/Item effect/Freeze enemeis effect
// order    ��ť��ʾ˳��
[CreateAssetMenu(fileName = "Freeze enemeis effect", menuName = "Data/Item effect/Freeze enemeis")]
/// <summary>
/// �������Ч��
/// </summary>
public class FreezeEnemies_Effect : ItemEffect
{
    //����ʱ��
    [SerializeField] private float duration;

    /// <summary>
    /// ִ�е�Ч��
    /// </summary>
    /// <param name="_transform"></param>
    public override void ExecuteEffect(Transform _transform)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        //Ѫ������10%�Ŵ���
        if (playerStats.currentHealth > playerStats.GetMaxHealthValue() * 0.1f)
        {
            return;
        }

        if (!Inventory.instance.CanUseArmor())
        {
            return;
        }

        //��ȡλ��Բ�������ڵ�����2D��Ϸ��ײ����б�  
        //��һ������ point	Բ�ε����ġ�
        //�ڶ������� radius	Բ�εİ뾶��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2);
        //������ײ����������Ƿ���ڵ��ˣ�������ڣ�����õ�������е�Damage����
        foreach (var hit in colliders)
        {
            hit.GetComponent<Enemy>().FreezeTimeFor(duration);    
        }
    }
}
