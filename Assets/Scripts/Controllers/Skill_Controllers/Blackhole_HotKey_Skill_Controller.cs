using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// 黑洞热键控制器
/// </summary>
public class Blackhole_HotKey_Skill_Controller : MonoBehaviour
{
    //渲染2D的精灵
    private SpriteRenderer spriteRenderer;
    //热键
    private KeyCode myHotKey;
    //显示的文本
    private TextMeshProUGUI myText;
    //敌人
    private Transform myEnemy;
    //黑洞控制器
    private Blackhole_Skill_Controller blackHole;

    //设置热键属性
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
            //添加在所有笼罩的目标,在敌人上方的热键上生成预制件
            blackHole.AddEnemyToList(myEnemy);

            myText.color = Color.clear;
            spriteRenderer.color = Color.clear;
        }
    }


}
