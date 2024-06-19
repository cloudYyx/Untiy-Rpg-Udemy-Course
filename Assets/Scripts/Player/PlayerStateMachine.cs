using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���״̬��
/// </summary>
public class PlayerStateMachine
{
    //��ȡ�ǹ��У��޸���˽�� == ֻ��
    public PlayerState currentState {  get; private set; }
    /// <summary>
    /// ��ʼ״̬
    /// </summary>
    /// <param name="_startState"></param>
    public void Initialize(PlayerState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }
    /// <summary>
    /// �ı�״̬
    /// </summary>
    /// <param name="_newState"></param>
    public void ChangeState(PlayerState _newState) 
    {  
        currentState.Exit();
        currentState = _newState; 
        currentState.Enter();
    }
}
