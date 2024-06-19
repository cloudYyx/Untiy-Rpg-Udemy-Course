using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
/// <summary>
/// ���������
/// </summary>
public class ShockStrike_Controller : MonoBehaviour
{
    //��������ͳ��
    [SerializeField] private CharacterStats characterState;
    //�ٶ�
    [SerializeField] private float speed;
    //�˺�
    private int damage;
    //����
    private Animator animator;
    //������
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

        //������ 1 ���� ����2��
        transform.position = Vector2.MoveTowards(transform.position,
            characterState.transform.position,speed * Time.deltaTime);

        transform.right = transform.position - characterState.transform.position;

        if (Vector2.Distance(transform.position,characterState.transform.position) < 0.1f)
        {
            //����ڸ����任�ı任λ��
            animator.transform.localPosition = new Vector3(0, 0.5f);
            //��ת����Ϊ0
            animator.transform.localRotation = Quaternion.identity;

            //����ڸ����任��ת�ı任��ת��
            transform.localRotation = Quaternion.identity;
            //����� GameObjects ������ı任���š�
            transform.localScale = new Vector3(3, 3);

            Invoke("DamageAndSelfDestroy", 0.2f);
            triggered = true;
            animator.SetTrigger("Hit");

        }
    }

    /// <summary>
    /// �˺������һ���
    /// </summary>
    private void DamageAndSelfDestroy()
    {
        characterState.ApplyShock(true);
        characterState.TakeDamage(damage);
        Destroy(gameObject, 0.4f);
    }
}
