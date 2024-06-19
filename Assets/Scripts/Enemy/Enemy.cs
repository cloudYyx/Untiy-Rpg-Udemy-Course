using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    //λ�����
    [SerializeField] protected LayerMask whatIsPlayer;

    //ѣ����Ϣ
    [Header("Stunned info")]
    //ѣ�γ���ʱ��
    public float stunDuration;
    //ѣ�η���
    public Vector2 stunDirection;
    //�Ƿ����ѣ��
    protected bool canBeStunned;
    //ѣ��ʱ����ͼ��
    [SerializeField] protected GameObject counterImage;

    [Header("Move info")]
    //�ƶ��ٶ�
    public float moveSpeed;
    //����ʱ��
    public float idleTime;
    //ս��ʱ��
    public float battleTime;
    //Ĭ���ƶ��ٶ�
    private float defaultMoveSpeed;

    [Header("Attack info")]
    //��������
    public float attackDistance;
    //����CD
    public float attackCooldown;
    //���ز������ڼ������    ��󹥻�ʱ��
    [HideInInspector]public float lastTimeAttacked;

    //״̬��
    public EnemyStateMachine stateMachine { get; private set; }
    //���Ķ�������
    public string lastAnimBoolname {  get; private set; }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        
        defaultMoveSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    /// <summary>
    /// �������һ����������
    /// </summary>
    /// <param name="_animBoolName"></param>
    public virtual void AssignLastAnimName(string _animBoolName)
    {
        lastAnimBoolname = _animBoolName;
    }

    /// <summary>
    /// ����ʵ�壬����Ԫ�ػ�ʹ��ɫ����
    /// </summary>
    /// <param name="_slowPercentage">�����ٷֱ�</param>
    /// <param name="_slowDuration">��������ʱ��</param>
    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);

        animator.speed = animator.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    /// <summary>
    /// ����Ĭ���ٶ�
    /// </summary>
    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
    }

    /// <summary>
    /// ����ʱ��
    /// </summary>
    /// <param name="_timeFrozen"></param>
    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            animator.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            animator.speed = 1;
        }
    }

    /// <summary>
    /// ���ö���ʱ��
    /// </summary>
    /// <param name="_duration"></param>
    public virtual void FreezeTimeFor(float _duration)
    {
        StartCoroutine(FreezeTimerCoroutine(_duration));
    }

    /// <summary>
    /// ���ᶨʱ��Эͬ����
    /// </summary>
    /// <param name="_seconds"></param>
    /// <returns></returns>
    protected virtual IEnumerator FreezeTimerCoroutine(float _seconds)
    {
        FreezeTime(true);

        yield return new WaitForSeconds(_seconds);

        FreezeTime(false);
    }

    #region Counter Attack Window
    /// <summary>
    /// �������أ����Է��� 
    /// </summary>
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }
    /// <summary>
    /// �������أ������Է��� 
    /// </summary>
    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }

    /// <summary>
    /// �ܷ�ѣ��
    /// </summary>
    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }
    #endregion
    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    /// <summary>
    /// 1.������ 2D �ռ��е���㡣2.��ʾ���߷����ʸ����3.���ߵ����Ͷ����롣4.�����������ڽ����ض����ϼ����ײ�塣
    /// </summary>
    /// <returns>���ع��ڹ���Ͷ��2D�����⵽�Ķ������Ϣ��</returns>
    public virtual RaycastHit2D IsPlayerDetected() =>
        Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir,
            15, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,
            new Vector3(transform.position.x + attackDistance * facingDir,
            transform.position.y));
    }
}
