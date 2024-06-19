using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 冰与火控制器
/// </summary>
public class IceAndFire_Controller : ThunderStrike_Controller
{
    /// <summary>
    //如果另一个碰撞器进入触发器，则调用这个
    /// </summary>
    /// <param name="collision"></param>
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
