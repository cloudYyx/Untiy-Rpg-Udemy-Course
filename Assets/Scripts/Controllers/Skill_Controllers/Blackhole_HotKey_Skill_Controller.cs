using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// �ڶ��ȼ�������
/// </summary>
public class Blackhole_HotKey_Skill_Controller : MonoBehaviour
{
    //��Ⱦ2D�ľ���
    private SpriteRenderer spriteRenderer;
    //�ȼ�
    private KeyCode myHotKey;
    //��ʾ���ı�
    private TextMeshProUGUI myText;
    //����
    private Transform myEnemy;
    //�ڶ�������
    private Blackhole_Skill_Controller blackHole;

    //�����ȼ�����
    public void SetupHotKey(KeyCode _myNewHotKey,Transform _myEnemy,Blackhole_Skill_Controller _myBlackHole)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();

        myEnemy = _myEnemy;
        blackHole = _myBlackHole;

        myHotKey = _myNewHotKey;
        myText.text = _myNewHotKey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            //������������ֵ�Ŀ��,�ڵ����Ϸ����ȼ�������Ԥ�Ƽ�
            blackHole.AddEnemyToList(myEnemy);

            myText.color = Color.clear;
            spriteRenderer.color = Color.clear;
        }
    }


}
