using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// �� Assets ����Ӳ˵�
// fileName ������Ϊ Ice and Fire effect�Ľű�
// menuName �˵���ť��Data/Item effect/Ice and Fire
// order    ��ť��ʾ˳��
[CreateAssetMenu(fileName = "Ice and Fire effect", menuName = "Data/Item effect/Ice and Fire")]
/// <summary>
/// ����Ч��
/// </summary>
public class IceAndFire_Effect : ItemEffect
{
    //����Ԥ����
    [SerializeField] private GameObject iceAndFirePrefab;
    //�����ٶ�
    [SerializeField] private float xVelocity;

    /// <summary>
    /// ִ�е�Ч��
    /// </summary>
    /// <param name="_respondPosition"></param>
    public override void ExecuteEffect(Transform _respondPosition)
    {
        Player player = PlayerManager.instance.player;

        bool thirdAttack = player.playerPrimaryAttack.comboCounter == 2;
        //��������ͨ�����򴥷���Ч
        if (thirdAttack)
        {
            //������ҳ�����ת��������ת
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _respondPosition.position, player.transform.rotation);

            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * player.facingDir, 0);

            Destroy(newIceAndFire,10);
        }
    }
}
