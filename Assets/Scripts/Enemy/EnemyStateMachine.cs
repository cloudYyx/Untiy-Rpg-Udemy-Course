using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����״̬��
/// </summary>
public class EnemyStateMachine 
{
    public EnemyState currentState { get; private set; }

    /// <summary>
    /// ��ʼ״̬
    /// </summary>
    /// <param name="_startState"></param>
    public void Initialize(EnemyState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    /// <summary>
    /// �ı�״̬
    /// </summary>
    /// <param name="_newState"></param>
    public void ChangeState(EnemyState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
