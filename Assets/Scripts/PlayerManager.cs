using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ҹ�����
/// </summary>
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Player player;
    //����
    public int currency;

    private void Awake()
    {
        //����ģʽ
        if (instance != null)
        {
            //�Ƴ�һ����Ϸ����
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    /// <summary>
    /// �Ƿ����㹻��Ǯ
    /// </summary>
    /// <param name="_price"></param>
    /// <returns></returns>
    public bool HaveEnoughMoney(int _price)
    {
        if (_price > currency)
        {
            Debug.Log("Ǯ����");
            return false;
        }

        currency = currency - _price;
        return true;
    }
}
