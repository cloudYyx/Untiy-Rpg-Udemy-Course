using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ������
/// </summary>
public enum SwordType
{
    Regular,//����
    Bounce,//����
    Pierce,//����
    Spin//��ת
}

/// <summary>
/// ������
/// </summary>
public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Bounce info")]
    //����������ť
    [SerializeField] private UI_SkillTreeSlot bounceUnlockButton;
    //�����Ծ��
    [SerializeField] private int bounceAmount;
    //����������
    [SerializeField] private float bounceGravity;
    //�����ٶ�
    [SerializeField] private float bounceSpeed;

    [Header("Peirce info")]
    //���̽�����ť
    [SerializeField] private UI_SkillTreeSlot pierceUnlockButton;
    //��󴩴���
    [SerializeField] private int pierceAmount;
    //��������
    [SerializeField] private float pierceGravity;

    [Header("Spin info")]
    //��ת������ť
    [SerializeField] private UI_SkillTreeSlot spinUnlockButton;
    [SerializeField] private float hitCooldown = 0.35f;
    //��Զ����
    [SerializeField] private float maxTravelDistance = 7;
    //��ת����ʱ��
    [SerializeField] private float spinDuration = 2;
    //��ת����
    [SerializeField] private float spinGravity = 1;

    [Header("Skill info")]
    //�ɽ�������ť
    [SerializeField] private UI_SkillTreeSlot swordUnlockButton;
    //�ɽ��Ƿ����
    public bool swordUnlocked { get; private set; }
    //����Ԥ����
    [SerializeField] private GameObject swordPrefab;
    //���䷽��
    [SerializeField] private Vector2 launchForce;
    //��������
    [SerializeField] private float swordGravity;
    //�����ٶ�
    [SerializeField] private float returnSpeed;
    //�������ʱ��
    [SerializeField] private float freezeTimeDuration;
    //���յķ���
    private Vector2 finalDir;

    [Header("Passive skills")]
    //ʱ��ֹͣ������ť
    [SerializeField] private UI_SkillTreeSlot timeStopUnlockButton;
    //ʱ��ֹͣ�Ƿ����
    public bool timeStopUnlocked { get; private set; }
    //���������ť
    [SerializeField] private UI_SkillTreeSlot vulnerabilityUnlockButton;
    //�����Ƿ����
    public bool vulnerabilityUnlocked { get; private set; }

    //Ŀ���
    [Header("Aim dots")]
    //����
    [SerializeField] private int numberOfDots;
    //�����֮��Ŀռ�
    [SerializeField] private float spaceBeetwenDots;
    //��Ԥ����
    [SerializeField] private GameObject dotPrefab;
    //�㸸��
    [SerializeField] private Transform dotsParent;
    //��
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
    /// ��������
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
    /// ���콣
    /// </summary>
    public void CreateSword()
    {
        //Object ʵ�����Ŀ�¡����
        //swordPrefab��Ҫ���Ƶ����ж���
        //position	�¶����λ�á�
        //rotation �¶���ķ���
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

    //��������
    #region Unlock region
    /// <summary>
    /// ��������
    /// </summary>
    private void UnlockBounce()
    {
        if (bounceUnlockButton.unlocked)
        {
            swordType = SwordType.Bounce;
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    private void UnlockPierce()
    {
        if (pierceUnlockButton.unlocked)
        {
            swordType= SwordType.Pierce;
        }
    }

    /// <summary>
    /// ������ת
    /// </summary>
    private void UnlockSpin()
    {
        if (spinUnlockButton.unlocked)
        {
            swordType = SwordType.Spin;
        }
    }

    /// <summary>
    /// �����ɽ�
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
    /// ����ʱ��ֹͣ
    /// </summary>
    private void UnlockTimeStop()
    {
        if (timeStopUnlockButton.unlocked)
        {
            timeStopUnlocked = true;
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    private void UnlockVulnerability()
    {
        if (vulnerabilityUnlockButton.unlocked)
        {
            vulnerabilityUnlocked = true;
        }
    }

    #endregion

    //Ŀ������
    #region Aim region
    /// <summary>
    /// Ŀ�귽��
    /// </summary>
    /// <returns></returns>
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        //�˷����������ǽ��ο���Input.mousePosition����Ļ����ϵת������������ϵ
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

    /// <summary>
    /// ���Ծ
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
    /// ���ɵ�
    /// </summary>
    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            //Object ʵ�����Ŀ�¡����
            //swordPrefab��Ҫ���Ƶ����ж���
            //position	�¶����λ�á�
            //rotation �¶���ķ���
            //parent	��ָ�����¶���ĸ�����
            dots[i] = Instantiate(dotPrefab, player.transform.position,
                //��λ��ת��ֻ����
                Quaternion.identity, dotsParent);
            //ͣ�û��߼�����Ϸ����
            dots[i].SetActive(false);
        }
    }

    /// <summary>
    /// ��λ��
    /// </summary>
    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + 
            //������ʽ �߶� = 1/2*�������ٶ�*ʱ��*ʱ��
            0.5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }
    #endregion
}
