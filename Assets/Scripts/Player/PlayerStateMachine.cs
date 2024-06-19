using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家状态机
/// </summary>
public class PlayerStateMachine
{
    //获取是公有，修改是私有 == 只读
    public PlayerState currentState {  get; private set; }
    /// <summary>
    /// 开始状态
    /// </summary>
    /// <param name="_startState"></param>
    public void Initialize(PlayerState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }
    /// <summary>
    /// 改变状态
    /// </summary>
    /// <param name="_newState"></param>
    public void ChangeState(PlayerState _newState) 
    {  
        currentState.Exit();
        currentState = _newState; 
        currentState.Enter();
    }
}
