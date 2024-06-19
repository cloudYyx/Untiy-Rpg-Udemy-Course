using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敌人状态机
/// </summary>
public class EnemyStateMachine 
{
    public EnemyState currentState { get; private set; }

    /// <summary>
    /// 开始状态
    /// </summary>
    /// <param name="_startState"></param>
    public void Initialize(EnemyState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    /// <summary>
    /// 改变状态
    /// </summary>
    /// <param name="_newState"></param>
    public void ChangeState(EnemyState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
