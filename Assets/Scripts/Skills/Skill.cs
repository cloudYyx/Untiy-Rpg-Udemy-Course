using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����
/// </summary>
public class Skill : MonoBehaviour
{
    //��ȴʱ��
    [SerializeField] protected float cooldown;
    //��ȴ��ʱ��
    protected float cooldownTimer;

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    /// <summary>
    /// �Ƿ�����ü���
    /// </summary>
    /// <returns></returns>
    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        return false;
    }

    /// <summary>
    /// ��һЩ�ض����ܵ�����
    /// </summary>
    public virtual void UseSkill()
    {

    }

    /// <summary>
    /// �ҵ�����ĵ���
    /// </summary>
    /// <returns></returns>
    protected virtual Transform FindClosestEnemy(Transform _checkTransform)
    {
        //��ȡλ��Բ�������ڵ�����2D��Ϸ��ײ����б�  
        //��һ������ point	Բ�ε����ġ�
        //�ڶ������� radius	Բ�εİ뾶��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25);
        //�������
        float closestDistance = Mathf.Infinity;
        //����Ĺ���
        Transform closestEnemy = null;

        //������ײ����������Ƿ���ڵ��ˣ�������ڣ�����õ�������е�Damage����
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                //����A��B֮��ľ���
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }
}
