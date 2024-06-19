using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家管理器
/// </summary>
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Player player;
    //货币
    public int currency;

    private void Awake()
    {
        //单例模式
        if (instance != null)
        {
            //移除一个游戏对象
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    /// <summary>
    /// 是否有足够的钱
    /// </summary>
    /// <param name="_price"></param>
    /// <returns></returns>
    public bool HaveEnoughMoney(int _price)
    {
        if (_price > currency)
        {
            Debug.Log("钱不够");
            return false;
        }

        currency = currency - _price;
        return true;
    }
}
