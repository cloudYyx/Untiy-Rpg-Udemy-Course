using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����״̬
/// </summary>
public class EnemyState
{
    //״̬��
    protected EnemyStateMachine stateMachine;
    //����
    protected Enemy enemyBase;
    //����
    protected Rigidbody2D rigidbody2D;
    //��������
    private string animBoolName;
    //������
    protected bool triggerCalled;
    //��ʱ��
    protected float stateTimer;

    public EnemyState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.enemyBase = enemyBase;
        this.animBoolName = animBoolName;
    }

    /// <summary>
    /// ����
    /// </summary>
    public virtual void Update()
    {
        //time -=  ��֮֡���ʱ��
        stateTimer -= Time.deltaTime;
    }

    /// <summary>
    /// ����
    /// </summary>
    public virtual void Enter()
    {
        triggerCalled = false;
        rigidbody2D = enemyBase.rb;
        enemyBase.animator.SetBool(animBoolName, true);
    }

    /// <summary>
    /// �˳�
    /// </summary>
    public virtual void Exit()
    {
        enemyBase.animator.SetBool(animBoolName, false);
        enemyBase.AssignLastAnimName(animBoolName);
    }

    /// <summary>
    /// ����������
    /// </summary>
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
