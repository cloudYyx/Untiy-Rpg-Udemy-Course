using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;
/// <summary>
/// �ڶ����ܿ�����
/// </summary>
public class Blackhole_Skill_Controller : MonoBehaviour
{
    //�ȼ�Ԥ����
    [SerializeField] private GameObject hotKeyPrefab;
    //��������
    [SerializeField] private List<KeyCode> keyCodeList;

    //������
    private float maxSize;
    //�����ٶ�
    private float growSpeed;
    //��С�ٶ�
    private float shrinkSpeed;
    //�ڶ���ʱ��
    private float blackholeTimer;

    //�Ƿ�����
    private bool canGrow = true;
    //�ܷ���С
    private bool canShrink;
    //����Ƿ�����
    private bool playerCanDisapear = true;
    //�ܷ񴴽��ȼ�o
    private bool canCreateHotKeys = true;
    //��¡�����Ƿ��ͷ�
    private bool cloneAttackReleased;

    //��������
    private int amountOfAttacks = 4;
    //��¡������ȴʱ��
    private float cloneAttackCooldown = 0.3f;
    //��¡������ʱ��
    private float cloneAttackTimer;

    //�������ֵ�Ŀ��
    private List<Transform> targets = new List<Transform>();
    //�����ȼ�
    private List<GameObject> createdHotKey = new List<GameObject>();

    //��ҿ����˳�״̬
    public bool playerCanExitState { get;private set; }

    /// <summary>
    /// ���úڶ�����
    /// ������,�����ٶ�,��С�ٶ�,��������,��¡������ȴʱ��
    /// </summary>
    public void SetupBlackhole(float _maxSize,float _growSpeed,float _shrinkSpeed,int _amountOfAttacks,float _cloneAttackCooldown,float _blackholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackholeTimer = _blackholeDuration;

        if (SkillManager.instance.clone.crystalInseadOfClone)
        {
            playerCanDisapear = false;
        }
    }

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if (blackholeTimer < 0)
        {
            blackholeTimer = Mathf.Infinity;

            if (targets.Count > 0)
            {
                ReleaseCloneAttack();
            }
            else
            {
                FinishBlackHoleAbility();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            //����� GameObjects ������ı任���š�
            //������ a �� b ֮�䰴 t �������Բ�ֵ��
            //������� t �����ڷ�Χ[0, 1] �ڡ�
            //�� t = 0 ʱ������ a ��
            //�� t = 1 ʱ������ b ��
            //�� t = 0.5 ʱ������ a �� b ���е㡣
            transform.localScale = Vector2.Lerp(transform.localScale,
                new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale,
                new Vector2(-1, 1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
            {
                canShrink = false;
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// �ͷſ�¡����
    /// </summary>
    private void ReleaseCloneAttack()
    {
        if (targets.Count < 0)
        {
            return;
        }

        DestroyHotKeys();
        cloneAttackReleased = true;
        canCreateHotKeys = false;

        if (playerCanDisapear)
        {
            playerCanDisapear = false;
            PlayerManager.instance.player.entityFX.MakeTransprent(true);
        }
        
    }

    /// <summary>
    /// ��¡�����߼�
    /// </summary>
    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased && amountOfAttacks > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);

            float xOffset;

            if (Random.Range(0, 100) > 50)
            {
                xOffset = 2;
            }
            else
            {
                xOffset = -2;
            }

            if (SkillManager.instance.clone.crystalInseadOfClone)
            {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            }
            else if(amountOfAttacks > 0)
            {
                SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));

            }

            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {                
                Invoke("FinishBlackHoleAbility", 0.5f);
            }
        }
    }

    /// <summary>
    /// ��ɺڶ�����
    /// </summary>
    private void FinishBlackHoleAbility()
    {
        DestroyHotKeys();
        playerCanExitState = true;
        canShrink = true;
        cloneAttackReleased = false;
    }

    /// <summary>
    /// ɾ���ȼ�
    /// </summary>
    private void DestroyHotKeys()
    {
        if (createdHotKey.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < createdHotKey.Count; i++)
        {
            Destroy(createdHotKey[i]);
        }
    }

    /// <summary>
    //�����һ����ײ�����봥��������������
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            
            CreateHotKey(collision);
        }
    }

    /// <summary>
    //�����һ����ײ���˳�����������������
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(false);
        }
    }
    //��д���ȼ�����ķ���
    //private void OnTriggerExit(Collider collision) => collision.GetComponent<Enemy>()?.FreezeTime(false);


    /// <summary>
    /// �����ȼ�
    /// </summary>
    /// <param name="collision"></param>
    private void CreateHotKey(Collider2D collision)
    {
        if(keyCodeList.Count <= 0)
        {
            return;
        }

        if (!canCreateHotKeys)
        {
            return;
        }

        //ʵ���� 
        //original Ҫ���Ƶ����ж���
        //position �¶����λ�á�
        //rotation �¶���ķ���
        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKey.Add(newHotKey);

        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);

        Blackhole_HotKey_Skill_Controller newHotKeyScript = newHotKey.GetComponent<Blackhole_HotKey_Skill_Controller>();

        newHotKeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform)
    {
        targets.Add(_enemyTransform);
    }
}
