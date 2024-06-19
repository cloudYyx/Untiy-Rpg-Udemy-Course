using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��¡���ܿ�����
/// </summary>
public class Clone_Skill_Controller : MonoBehaviour
{
    //���
    private Player player;
    //��Ⱦ2Dͼ�εľ���
    private SpriteRenderer spriteRenderer;
    //����ʦ
    private Animator animator;
    //��ɫʧȥ��ʱ��
    [SerializeField] private float colorLoosingSpeed;

    //��¡��ʱ��
    private float cloneTimer;
    //��������
    private float attackMultiplier;
    //�������
    [SerializeField] private Transform attackCheck;
    //�������뾶
    [SerializeField] private float attackCheckRadius = 0.8f;
    //����Ĺ���
    private Transform closestEnemy;
    //�泯�� 1Ϊ�� -1Ϊ��
    private int facingDir = 1;

    //�ܷ��ƿ�¡
    private bool canDuplicateClone;
    //���ƵĻ���
    private float chanceToduplicate;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            //���ĸ�������͸����
            spriteRenderer.color = new Color(1, 1, 1,
                spriteRenderer.color.a - (Time.deltaTime * colorLoosingSpeed));
            if (spriteRenderer.color.a <= 0)
            {
                //�Ƴ���Ϸ����
                Destroy(gameObject);
            }
        }
    }

    //����Ԥ�����¡λ��,_offsetƫ����
    public void SetupClone(Transform _newTransform,float _cloneDuration,bool _canAttack,Vector3 _offset,Transform _closestEnemy,bool _canDuplicate,float _chanceToduplicate,Player _player,float _attackMultiplier)
    {
        if (_canAttack)
        {
            animator.SetInteger("AttackNumber", Random.Range(1, 3));
        }

        attackMultiplier = _attackMultiplier;
        player = _player;
        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;

        closestEnemy = _closestEnemy;
        canDuplicateClone = _canDuplicate;
        chanceToduplicate = _chanceToduplicate;

        FaceClosesTarget();
    }

    /// <summary>
    /// ��������������
    /// �ڶ�������-�����ұ�-����¼�����ô˷��������ô�������Ϊture
    /// </summary>
    private void AnimationTrigger()
    {
        cloneTimer = -0.1f;
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
        //������ײ����������Ƿ���ڵ��ˣ�������ڣ�����õ�������е�Damage����
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                //player.characterState.DoDamage(hit.GetComponent<CharacterStats>());
                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();

                playerStats.CloneDoDamage(enemyStats, attackMultiplier);

                if (player.skillManager.clone.canApplyOnHitEffect)
                {
                    ItemData_Equipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);

                    if (weaponData != null)
                    {
                        weaponData.Effect(hit.transform);
                    }
                }

                if (canDuplicateClone)
                {
                    if (Random.Range(0,100) < chanceToduplicate)
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(0.5f * facingDir, 0));
                    }
                }
            }
        }
    }

    /// <summary>
    /// �泯�����Ŀ��
    /// </summary>
    private void FaceClosesTarget()
    {
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
