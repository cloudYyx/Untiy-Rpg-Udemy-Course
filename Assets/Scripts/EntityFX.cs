using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����Ч��
/// </summary>
public class EntityFX : MonoBehaviour
{
    //2D�ľ�����Ⱦ��
    private SpriteRenderer spriteRenderer;

    [Header("Flash FX")]
    //�������ʱ��
    [SerializeField] private float flashDuration;
    //�Ȳ���
    [SerializeField] private Material hitMat;
    //ԭʼ����
    private Material originalMat;

    //Ԫ����ɫ
    [Header("Element colors")]
    //��������ɫ
    [SerializeField] private Color[] chillColor;
    //��ȼ����ɫ
    [SerializeField] private Color[] igniteColor;
    //�𾪵���ɫ
    [SerializeField] private Color[] shockColor;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        //���ط������Ⱦ���ĵ�һ��ʵ�������ʡ�
        originalMat = spriteRenderer.material;
    }

    /// <summary>
    /// ��͸��
    /// </summary>
    public void MakeTransprent(bool _transprent)
    {
        if (_transprent)
        {
            spriteRenderer.color = Color.clear;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    /// <summary>
    /// Э�� ������Ч
    /// </summary>
    /// <returns></returns>
    private IEnumerator FlashFX()
    {
        spriteRenderer.material = hitMat;
        Color currentColor = spriteRenderer.color;
        spriteRenderer.color = Color.white;

        //�ȴ��������ʱ�������һ��ָ����ʱ���ӳ�֮�����ִ�У������е�Update������ɵ��õ���һ֮֡�������ʱ����ܵ�Time.timeScale��Ӱ�죩;
        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.color = currentColor;
        spriteRenderer.material = originalMat;
    }
   
    /// <summary>
    /// �����˸
    /// </summary>
    private void RedColorBlink()
    {
        if (spriteRenderer.color != Color.white)
        {
            spriteRenderer.color = Color.white;
        }
        else
        {
            spriteRenderer.color = Color.red;
        }
    }

    /// <summary>
    /// ȡ����ɫ����
    /// </summary>
    private void CancelColorChange()
    {
        //ȡ�����MonoBehaviour������Invoke����
        CancelInvoke();
        spriteRenderer.color = Color.white;
    }

    /// <summary>
    /// ��ȼ��Ч
    /// </summary>
    /// <param name="_seconds"></param>
    public void IgniteFxFor(float _seconds)
    {
        //public void InvokeRepeating (string methodName, float time, float repeatRate);
        //�� time ������ methodName ������Ȼ��ÿ repeatRate �����һ�Ρ�
        InvokeRepeating("IgniteColorFx",0,0.3f);
        Invoke("CancelColorChange", _seconds);
    }

    /// <summary>
    /// ������Ч
    /// </summary>
    /// <param name="_seconds"></param>
    public void ChillFxFor(float _seconds)
    {
        //public void InvokeRepeating (string methodName, float time, float repeatRate);
        //�� time ������ methodName ������Ȼ��ÿ repeatRate �����һ�Ρ�
        InvokeRepeating("ChillColorFx", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }

    /// <summary>
    /// ����Ч
    /// </summary>
    /// <param name="_seconds"></param>
    public void ShorckFxFor(float _seconds)
    {
        //public void InvokeRepeating (string methodName, float time, float repeatRate);
        //�� time ������ methodName ������Ȼ��ÿ repeatRate �����һ�Ρ�
        InvokeRepeating("ShorckColorFx", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }

    /// <summary>
    /// ��ȼ��ɫ��Ч
    /// </summary>
    private void IgniteColorFx()
    {
        if (spriteRenderer.color != igniteColor[0])
        {
            spriteRenderer.color = igniteColor[0];
        }
        else
        {
            spriteRenderer.color = igniteColor[1];
        }
    }

    /// <summary>
    /// ����ɫ��Ч
    /// </summary>
    private void ShorckColorFx()
    {
        if (spriteRenderer.color != shockColor[0])
        {
            spriteRenderer.color = shockColor[0];
        }
        else
        {
            spriteRenderer.color = shockColor[1];
        }
    }

    /// <summary>
    /// ������ɫ��Ч
    /// </summary>
    private void ChillColorFx()
    {
        if (spriteRenderer.color != chillColor[0])
        {
            spriteRenderer.color = chillColor[0];
        }
        else
        {
            spriteRenderer.color = chillColor[1];
        }
    }
}
