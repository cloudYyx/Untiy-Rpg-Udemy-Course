using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 在 Assets 下添加菜单
// fileName 生成名为 Ice and Fire effect的脚本
// menuName 菜单按钮名Data/Item effect/Ice and Fire
// order    按钮显示顺序
[CreateAssetMenu(fileName = "Ice and Fire effect", menuName = "Data/Item effect/Ice and Fire")]
/// <summary>
/// 冰火效果
/// </summary>
public class IceAndFire_Effect : ItemEffect
{
    //冰火预制体
    [SerializeField] private GameObject iceAndFirePrefab;
    //刚体速度
    [SerializeField] private float xVelocity;

    /// <summary>
    /// 执行的效果
    /// </summary>
    /// <param name="_respondPosition"></param>
    public override void ExecuteEffect(Transform _respondPosition)
    {
        Player player = PlayerManager.instance.player;

        bool thirdAttack = player.playerPrimaryAttack.comboCounter == 2;
        //第三下普通攻击则触发特效
        if (thirdAttack)
        {
            //根据玩家朝向旋转，右则右转
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _respondPosition.position, player.transform.rotation);

            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * player.facingDir, 0);

            Destroy(newIceAndFire,10);
        }
    }
}
