using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
/// <summary>
/// 冲击控制器
/// </summary>
public class ShockStrike_Controller : MonoBehaviour
{
    //人物属性统计
    [SerializeField] private CharacterStats characterState;
    //速度
    [SerializeField] private float speed;
    //伤害
    private int damage;
    //动画
    private Animator animator;
    //触发器
    private bool triggered;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Setup(int _damage, CharacterStats _characterState)
    {
        damage = _damage;
        characterState = _characterState;
    }

    private void Update()
    {
        if (!characterState)
        {
            return;
        }

        if (triggered)
        {
            return;
        }

        //将属性 1 移向 属性2。
        transform.position = Vector2.MoveTowards(transform.position,
            characterState.transform.position,speed * Time.deltaTime);

        transform.right = transform.position - characterState.transform.position;

        if (Vector2.Distance(transform.position,characterState.transform.position) < 0.1f)
        {
            //相对于父级变换的变换位置
            animator.transform.localPosition = new Vector3(0, 0.5f);
            //旋转设置为0
            animator.transform.localRotation = Quaternion.identity;

            //相对于父级变换旋转的变换旋转。
            transform.localRotation = Quaternion.identity;
            //相对于 GameObjects 父对象的变换缩放。
            transform.localScale = new Vector3(3, 3);

            Invoke("DamageAndSelfDestroy", 0.2f);
            triggered = true;
            animator.SetTrigger("Hit");

        }
    }

    /// <summary>
    /// 伤害和自我毁灭
    /// </summary>
    private void DamageAndSelfDestroy()
    {
        characterState.ApplyShock(true);
        characterState.TakeDamage(damage);
        Destroy(gameObject, 0.4f);
    }
}
