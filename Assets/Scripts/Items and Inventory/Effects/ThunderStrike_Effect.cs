using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// �� Assets ����Ӳ˵�
// fileName ������Ϊ Thunder strike effect�Ľű�
// menuName �˵���ť��Data/Item effect/Thunder strike
// order    ��ť��ʾ˳��
[CreateAssetMenu(fileName = "Thunder strike effect", menuName = "Data/Item effect/Thunder strike")]
/// <summary>
/// �׻�Ч��
/// </summary>
public class ThunderStrike_Effect : ItemEffect
{
    //�׻�Ԥ����
    [SerializeField] private GameObject thunderStrikePrefab;

    /// <summary>
    /// ִ�е�Ч��
    /// </summary>
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        //Object ʵ�����Ŀ�¡����
        //swordPrefab��Ҫ���Ƶ����ж���
        //position	�¶����λ�á�
        //rotation �¶���ķ���
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, _enemyPosition.position, Quaternion.identity);

        Destroy(newThunderStrike,1f);
    }
}
