using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ���˶���������  
/// �ѽű���ӽ�����Ԥ����
/// </summary>
public class Enemy_SkeletonAnimationTriggers : MonoBehaviour
{
    /// <summary>
    /// ���봴��һ����������������������������������������ȡ�������
    /// </summary>
    private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

    /// <summary>
    /// �ڶ�������-�����ұ�-����¼�����ô˷��������ô�������Ϊture
    /// </summary>
    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
        //������ײ����������Ƿ���ڵ��ˣ�������ڣ�����õ�������е�Damage����
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats target = hit.GetComponent<PlayerStats>();

                enemy.characterState.DoDamage(target);
            }
        }
    }

    /// <summary>
    /// �������������� �� ������ķ����ǹ�
    /// �ڶ�������-�����ұ�-����¼�����ô˷��������ô�������Ϊture
    /// </summary>
    protected void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    protected void CloseCounterAttackWindow() => enemy.CloseCounterAttackWindow();
}
