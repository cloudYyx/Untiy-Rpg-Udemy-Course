using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 闪光效果
/// </summary>
public class EntityFX : MonoBehaviour
{
    //2D的精灵渲染器
    private SpriteRenderer spriteRenderer;

    [Header("Flash FX")]
    //闪光持续时间
    [SerializeField] private float flashDuration;
    //热材料
    [SerializeField] private Material hitMat;
    //原始材料
    private Material originalMat;

    //元素颜色
    [Header("Element colors")]
    //冰冻的颜色
    [SerializeField] private Color[] chillColor;
    //点燃的颜色
    [SerializeField] private Color[] igniteColor;
    //震惊的颜色
    [SerializeField] private Color[] shockColor;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        //返回分配给渲染器的第一个实例化材质。
        originalMat = spriteRenderer.material;
    }

    /// <summary>
    /// 变透明
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
    /// 协程 闪光特效
    /// </summary>
    /// <returns></returns>
    private IEnumerator FlashFX()
    {
        spriteRenderer.material = hitMat;
        Color currentColor = spriteRenderer.color;
        spriteRenderer.color = Color.white;

        //等待闪光持续时间结束，一段指定的时间延迟之后继续执行，在所有的Update函数完成调用的那一帧之后（这里的时间会受到Time.timeScale的影响）;
        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.color = currentColor;
        spriteRenderer.material = originalMat;
    }
   
    /// <summary>
    /// 红白闪烁
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
    /// 取消颜色更改
    /// </summary>
    private void CancelColorChange()
    {
        //取消这个MonoBehaviour的所有Invoke调用
        CancelInvoke();
        spriteRenderer.color = Color.white;
    }

    /// <summary>
    /// 点燃特效
    /// </summary>
    /// <param name="_seconds"></param>
    public void IgniteFxFor(float _seconds)
    {
        //public void InvokeRepeating (string methodName, float time, float repeatRate);
        //在 time 秒后调用 methodName 方法，然后每 repeatRate 秒调用一次。
        InvokeRepeating("IgniteColorFx",0,0.3f);
        Invoke("CancelColorChange", _seconds);
    }

    /// <summary>
    /// 冻结特效
    /// </summary>
    /// <param name="_seconds"></param>
    public void ChillFxFor(float _seconds)
    {
        //public void InvokeRepeating (string methodName, float time, float repeatRate);
        //在 time 秒后调用 methodName 方法，然后每 repeatRate 秒调用一次。
        InvokeRepeating("ChillColorFx", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }

    /// <summary>
    /// 震惊特效
    /// </summary>
    /// <param name="_seconds"></param>
    public void ShorckFxFor(float _seconds)
    {
        //public void InvokeRepeating (string methodName, float time, float repeatRate);
        //在 time 秒后调用 methodName 方法，然后每 repeatRate 秒调用一次。
        InvokeRepeating("ShorckColorFx", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }

    /// <summary>
    /// 点燃颜色特效
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
    /// 震惊颜色特效
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
    /// 冻结颜色特效
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
