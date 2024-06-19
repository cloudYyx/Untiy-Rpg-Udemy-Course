using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��Ҷ���������
/// �ѽű���ӽ�����Ԥ����
/// </summary>
public class PlayerAnimationTriggers : MonoBehaviour
{
    /// <summary>
    /// ���봴��һ����������������������������������������ȡ�������
    /// </summary>
    private Player player => GetComponentInParent<Player>();

    /// <summary>
    /// ��������������
    /// �ڶ�������-�����ұ�-����¼�����ô˷��������ô�������Ϊture
    /// </summary>
    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    /// <summary>
    /// ��������������
    /// �ڶ�������-�����ұ�-����¼�����ô˷��������ô�������Ϊture
    /// </summary>
    private void AttackTrigger()
    {
        //��ȡλ��Բ�������ڵ�����2D��Ϸ��ײ����б�  
        //��һ������ point	Բ�ε����ġ�
        //�ڶ������� radius	Բ�εİ뾶��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        //������ײ����������Ƿ���ڵ��ˣ�������ڣ�����õ�������е�Damage����
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();

                if (_target != null)
                {
                    player.characterState.DoDamage(_target);
                }

                WeaponEffect(_target);
            }
        }
    }

    /// <summary>
    /// ������Ч��
    /// </summary>
    private void WeaponEffect(EnemyStats _target)
    {
        ItemData_Equipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);

        if (weaponData != null)
        {
            weaponData.Effect(_target.transform);
        }
    }

    /// <summary>
    /// �ӽ�������
    /// </summary>
    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
