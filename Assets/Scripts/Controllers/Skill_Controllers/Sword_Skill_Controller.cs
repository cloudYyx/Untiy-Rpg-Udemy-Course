using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
/// <summary>
/// �����ܿ�����
/// </summary>
public class Sword_Skill_Controller : MonoBehaviour
{
    //����ʦ
    private Animator animator;
    //����
    private Rigidbody2D rb;
    //Բ����ײ��
    private CircleCollider2D circleCollider2D;
    //���
    private Player player;
    //�ܷ���ת
    private bool canRotate = true;
    //�ܷ񷵻�
    private bool isReturning;

    //�������ʱ��
    private float freezeTimeDuration;
    //�����ٶ�
    private float returnSpeed = 12;

    [Header("Pierce info")]
    //��󴩴���
    private float pierceAmount;

    [Header("Bounce info")]
    //�Ƿ�����Ծ
    private bool isBouncing;
    //�����Ծ��
    private int bounceAmount;
    //���е���Ŀ��
    private List<Transform> enemyTarget;
    //Ŀ��ָ��
    private int targetIndex;
    //�����ٶ�
    private float bounceSpeed;

    [Header("Spin info")]
    //��Զ����
    private float maxTravelDistance;
    //��ת����ʱ��
    private float spinDuration;
    //��ת��ʱ��
    private float spinTimer;
    //�Ƿ�ֹͣ
    private bool wasStopped;
    //�Ƿ�����ת
    private bool isSpinning;
    //��ת����
    private float spinDirection;

    //���ж�ʱ��
    private float hitTimer;
    //������ȴ
    private float hitCooldown;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    /// <summary>
    /// �ݻ�
    /// </summary>
    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    //���ý����� (���������̶�)
    public void SetupSword(Vector2 _dir,float _gravityScale,Player _player,float _freezeTimeDuration,float _returnSpeed)
    {
        player = _player;
        freezeTimeDuration = _freezeTimeDuration;
        returnSpeed = _returnSpeed;
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;

        if (pierceAmount <= 0)
        {
            animator.SetBool("Rotation", true);
        }
        //�ڸ�������С������ֵ����󸡵���ֵ֮��ǯ�Ƹ���ֵ���������ֵ����Сֵ�����ֵ��Χ�ڣ��򷵻ظ���ֵ��
        //��������ĸ���ֵС����Сֵ���򷵻���Сֵ��
        //�������ֵ�������ֵ���򷵻����ֵ��ʹ�á��оߡ���ֵ����Ϊ����Сֵ�����ֵ����ķ�Χ��
        //�����Сֵ�������ֵ���򷵻�δ�����ֵ��
        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        Invoke("DestroyMe", 7);
    }

    //���ý���Ծ�������Ƿ�����Ծ�������Ծ����
    public void SetupBounce(bool _isBouncing, int _amountOfBounce,float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountOfBounce;
        bounceSpeed = _bounceSpeed;

        enemyTarget = new List<Transform>();
    }

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetupSpin(bool _isSpinning,float _maxTravelDistance,float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }

    /// <summary>
    /// ���ؽ�
    /// </summary>
    public void ReturnSword()
    {
        //������������ת���˶�
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //�������Ӧ���������������
        //rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }

    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.velocity;
        }

        if (isReturning)
        {
            //���� ����1 ���� ����2��
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.transform.position,
                returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.CatchTheSword();
            }
        }

        BounceLogic();
        SpinLogic();
    }

    /// <summary>
    /// ��ת�߼�
    /// </summary>
    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;
                //���� ����1 ���� ����2
                //transform.position = Vector2.MoveTowards(transform.position,
                //    new Vector2(transform.position.x + spinDirection,
                //    transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    //�����봥������������һ����Χ
                    //��ȡλ��Բ�������ڵ�����2D��Ϸ��ײ����б�  
                    //��һ������ point	Բ�ε����ġ�
                    //�ڶ������� radius	Բ�εİ뾶��
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                        {
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// ֹͣ��ת
    /// </summary>
    private void StopWhenSpinning()
    {
        wasStopped = true;
        //����X��Y���˶�
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }
    
    /// <summary>
    /// �����߼�
    /// </summary>
    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            //���� ����1 ���� ����2
            transform.position = Vector2.MoveTowards(
                transform.position, enemyTarget[targetIndex].position,
                bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < 0.1f)
            {
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());

                targetIndex++;
                bounceAmount--;

                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }

    //�����һ����ײ�����봥��������������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
        {
            return;
        }

        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }

        SetupTargetForBounce(collision);

        StuckInto(collision);
    }

    /// <summary>
    /// �ý������˺�����
    /// </summary>
    /// <param name="enemy"></param>
    private void SwordSkillDamage(Enemy enemy)
    {
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
        player.characterState.DoDamage(enemyStats);

        if (player.skillManager.sword.timeStopUnlocked)
        {
            enemy.FreezeTimeFor(freezeTimeDuration);
        }

        if (player.skillManager.sword.vulnerabilityUnlocked)
        {
            enemyStats.MakeVulnerableFor(freezeTimeDuration);
        }

        //���ݻ�����ӳ������Ч
        ItemData_Equipment equipmentAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

        if (equipmentAmulet != null)
        {
            equipmentAmulet.Effect(enemy.transform);
        }
    }

    //Ϊ��������Ŀ��
    private void SetupTargetForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                //�����봥������������һ����Χ
                //��ȡλ��Բ�������ڵ�����2D��Ϸ��ײ����б�  
                //��һ������ point	Բ�ε����ġ�
                //�ڶ������� radius	Բ�εİ뾶��
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        //����Χ���е��ˣ�����ӽ�����Ŀ�꼯����
                        enemyTarget.Add(hit.transform);
                    }
                }
            }
        }
    }

    //����
    private void StuckInto(Collider2D collision)
    {
        if(pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        canRotate = false;
        //�ر���ײ��
        circleCollider2D.enabled = false;
        //�������Ӧ���������������
        rb.isKinematic = true;
        //���Ƹ� Rigidbody2D ��ģ�����ɶ� = �������ᶳ����ת���ƶ���
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        if (isBouncing && enemyTarget.Count > 0)
        {
            return;
        }

        animator.SetBool("Rotation", false);
        //������Ϊ����ײ���������    
        transform.parent = collision.transform;
    }
}
