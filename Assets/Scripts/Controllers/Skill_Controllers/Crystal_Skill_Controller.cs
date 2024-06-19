using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
/// <summary>
/// ˮ�����ܿ�����
/// </summary>
public class Crystal_Skill_Controller : MonoBehaviour
{
    //����
    private Animator animator => GetComponent<Animator>();
    //Բ����ײ��
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    //���
    private Player player;

    //ˮ�����ڶ�ʱ��
    private float crystalExistTimer;

    //�ܷ�ը
    private bool canExplode;
    //�ܷ��ƶ�
    private bool canMove;
    //�ƶ��ٶ�
    private float moveSpeed;

    //�ܷ�����
    private bool canGrow;
    //�����ٶ�
    private float growSpeed = 5;

    //�����Ŀ��
    private Transform closestTarget;
    //λ�����
    [SerializeField] private LayerMask whatIsEnemy;

    /// <summary>
    /// ����ˮ������
    /// </summary>
    /// <param name="_crystalDuration"></param>
    public void SetupCrystal(float _crystalDuration,bool _canExplode,bool _canMove,float _moveSpeed,Transform _closestTarget, Player _player)
    {
        crystalExistTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestTarget = _closestTarget;
        player = _player;
    }

    /// <summary>
    /// ѡ������ĵ���
    /// </summary>
    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackhole.GetBlackholeRadius();

        //��ȡλ��Բ�������ڵ�����2D��Ϸ��ײ����б�  
        //��һ������ point	Բ�ε����ġ�
        //�ڶ������� radius	Բ�εİ뾶��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);

        if (colliders.Length > 0)
        {
            closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
        }
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;

        if (crystalExistTimer < 0)
        {
            FinishCrystal();
        }

        if (canMove)
        {
            if (closestTarget != null)
            {
                //������ 1 ���� ����2��
                transform.position = Vector2.MoveTowards(transform.position,
                    closestTarget.position, moveSpeed * Time.deltaTime);

                if (Vector2.Distance(transform.position, closestTarget.position) < 1)
                {
                    FinishCrystal();
                    canMove = false;
                }
            }
        }

        if (canGrow)
        {
            //����� GameObjects ������ı任���š�
            //������ a �� b ֮�䰴 t �������Բ�ֵ��
            //������� t �����ڷ�Χ[0, 1] �ڡ�
            //�� t = 0 ʱ������ a ��
            //�� t = 1 ʱ������ b ��
            //�� t = 0.5 ʱ������ a �� b ���е㡣
            transform.localScale = Vector2.Lerp(transform.localScale,
                new Vector2(3, 3), growSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// ˮ����ը�¼�
    /// </summary>
    private void AnimationExplodeEvent()
    {
        //��ȡλ��Բ�������ڵ�����2D��Ϸ��ײ����б�  
        //��һ������ point	Բ�ε����ġ�
        //�ڶ������� radius	Բ�εİ뾶��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);
        //������ײ����������Ƿ���ڵ��ˣ�������ڣ�����õ�������е�Damage����
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                player.characterState.DoMagicalDamage(hit.GetComponent<CharacterStats>());
                //���ݻ�����ӳ������Ч
                ItemData_Equipment equipmentAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

                if (equipmentAmulet != null)
                {
                    equipmentAmulet.Effect(hit.transform);
                }
            }
        }
    }

    /// <summary>
    /// ˮ������
    /// </summary>
    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            animator.SetTrigger("Explode");
        }
        else
        {
            SelfDestroy();
        }
    }

    /// <summary>
    /// ���һ���
    /// </summary>
    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
