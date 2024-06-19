using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 剑类型
/// </summary>
public enum SwordType
{
    Regular,//常规
    Bounce,//弹跳
    Pierce,//穿刺
    Spin//旋转
}

/// <summary>
/// 剑技能
/// </summary>
public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Bounce info")]
    //弹跳解锁按钮
    [SerializeField] private UI_SkillTreeSlot bounceUnlockButton;
    //最大跳跃量
    [SerializeField] private int bounceAmount;
    //弹跳的重力
    [SerializeField] private float bounceGravity;
    //弹跳速度
    [SerializeField] private float bounceSpeed;

    [Header("Peirce info")]
    //穿刺解锁按钮
    [SerializeField] private UI_SkillTreeSlot pierceUnlockButton;
    //最大穿刺量
    [SerializeField] private int pierceAmount;
    //穿刺重力
    [SerializeField] private float pierceGravity;

    [Header("Spin info")]
    //旋转解锁按钮
    [SerializeField] private UI_SkillTreeSlot spinUnlockButton;
    [SerializeField] private float hitCooldown = 0.35f;
    //最远距离
    [SerializeField] private float maxTravelDistance = 7;
    //旋转持续时间
    [SerializeField] private float spinDuration = 2;
    //旋转重力
    [SerializeField] private float spinGravity = 1;

    [Header("Skill info")]
    //飞剑解锁按钮
    [SerializeField] private UI_SkillTreeSlot swordUnlockButton;
    //飞剑是否解锁
    public bool swordUnlocked { get; private set; }
    //剑的预制体
    [SerializeField] private GameObject swordPrefab;
    //发射方向
    [SerializeField] private Vector2 launchForce;
    //剑的重力
    [SerializeField] private float swordGravity;
    //返回速度
    [SerializeField] private float returnSpeed;
    //冻结持续时间
    [SerializeField] private float freezeTimeDuration;
    //最终的方向
    private Vector2 finalDir;

    [Header("Passive skills")]
    //时间停止解锁按钮
    [SerializeField] private UI_SkillTreeSlot timeStopUnlockButton;
    //时间停止是否解锁
    public bool timeStopUnlocked { get; private set; }
    //弱点解锁按钮
    [SerializeField] private UI_SkillTreeSlot vulnerabilityUnlockButton;
    //弱点是否解锁
    public bool vulnerabilityUnlocked { get; private set; }

    //目标点
    [Header("Aim dots")]
    //点数
    [SerializeField] private int numberOfDots;
    //点与点之间的空间
    [SerializeField] private float spaceBeetwenDots;
    //点预制体
    [SerializeField] private GameObject dotPrefab;
    //点父级
    [SerializeField] private Transform dotsParent;
    //点
    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();

        GenerateDots();

        SetupGraivty();

        bounceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBounce);
        pierceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockPierce);
        spinUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSpin);
        swordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSword);
        timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        vulnerabilityUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockVulnerability);
    }

    /// <summary>
    /// 设置重力
    /// </summary>
    private void SetupGraivty()
    {
        if (swordType == SwordType.Bounce)
        {
            swordGravity = bounceGravity;
        }
        else if (swordType == SwordType.Pierce)
        {
            swordGravity = pierceGravity;
        }
        else if (swordType == SwordType.Spin)
        {
            swordGravity = spinGravity;
        }
    }
    
    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x,
                AimDirection().normalized.y * launchForce.y);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for(int i = 0; i < dots.Length; i++) 
            {
                dots[i].transform.position = DotsPosition(i * spaceBeetwenDots);
            }
        }
    }

    /// <summary>
    /// 创造剑
    /// </summary>
    public void CreateSword()
    {
        //Object 实例化的克隆对象。
        //swordPrefab是要复制的现有对象。
        //position	新对象的位置。
        //rotation 新对象的方向。
        GameObject newSword = Instantiate(swordPrefab, 
            player.transform.position, 
            transform.rotation);

        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();

        if (swordType == SwordType.Bounce)
        {
            newSwordScript.SetupBounce(true, bounceAmount,bounceSpeed);
        }
        else if (swordType == SwordType.Pierce)
        {
            newSwordScript.SetupPierce(pierceAmount);
        }
        else if (swordType == SwordType.Spin)
        {
            newSwordScript.SetupSpin(true,maxTravelDistance,spinDuration,hitCooldown);
        }

        newSwordScript.SetupSword(finalDir, swordGravity,player,freezeTimeDuration,returnSpeed);

        player.AssignNewSword(newSword);

        DotsActive(false);
    }

    //解锁区域
    #region Unlock region
    /// <summary>
    /// 解锁弹跳
    /// </summary>
    private void UnlockBounce()
    {
        if (bounceUnlockButton.unlocked)
        {
            swordType = SwordType.Bounce;
        }
    }

    /// <summary>
    /// 解锁穿刺
    /// </summary>
    private void UnlockPierce()
    {
        if (pierceUnlockButton.unlocked)
        {
            swordType= SwordType.Pierce;
        }
    }

    /// <summary>
    /// 解锁旋转
    /// </summary>
    private void UnlockSpin()
    {
        if (spinUnlockButton.unlocked)
        {
            swordType = SwordType.Spin;
        }
    }

    /// <summary>
    /// 解锁飞剑
    /// </summary>
    private void UnlockSword()
    {
        if (swordUnlockButton.unlocked)
        {
            swordUnlocked = true;
            swordType = SwordType.Regular;
        }
    }

    /// <summary>
    /// 解锁时间停止
    /// </summary>
    private void UnlockTimeStop()
    {
        if (timeStopUnlockButton.unlocked)
        {
            timeStopUnlocked = true;
        }
    }

    /// <summary>
    /// 解锁弱点
    /// </summary>
    private void UnlockVulnerability()
    {
        if (vulnerabilityUnlockButton.unlocked)
        {
            vulnerabilityUnlocked = true;
        }
    }

    #endregion

    //目标区域
    #region Aim region
    /// <summary>
    /// 目标方向
    /// </summary>
    /// <returns></returns>
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        //此方法的作用是将参考点Input.mousePosition从屏幕坐标系转换到世界坐标系
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

    /// <summary>
    /// 点活跃
    /// </summary>
    /// <param name="_isActive"></param>
    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    /// <summary>
    /// 生成点
    /// </summary>
    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            //Object 实例化的克隆对象。
            //swordPrefab是要复制的现有对象。
            //position	新对象的位置。
            //rotation 新对象的方向。
            //parent	将指定给新对象的父对象。
            dots[i] = Instantiate(dotPrefab, player.transform.position,
                //单位旋转（只读）
                Quaternion.identity, dotsParent);
            //停用或者激活游戏对象
            dots[i].SetActive(false);
        }
    }

    /// <summary>
    /// 点位置
    /// </summary>
    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + 
            //重力公式 高度 = 1/2*重力加速度*时间*时间
            0.5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }
    #endregion
}
