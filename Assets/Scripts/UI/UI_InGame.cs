using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ��Ϸ�е�UI
/// </summary>
public class UI_InGame : MonoBehaviour
{
    //���״̬
    [SerializeField] private PlayerStats playerStats;
    //����һ��������
    [SerializeField] private Slider slider;
    
    //���ͼ��
    [SerializeField] private Image dashImage;

    void Start()
    {
        if (playerStats != null)
        {
            playerStats.onHealthChanged += UpdateHealthUI;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SetCooldownOf(dashImage);
        }
    }

    /// <summary>
    /// ����UI�Ľ���ֵ
    /// </summary>
    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }

    /// <summary>
    /// ������ȴ
    /// </summary>
    /// <param name="_image"></param>
    private void SetCooldownOf(Image _image)
    {
        //��ΧΪ 0-1��0 Ϊ����ʹ�ã�1 Ϊ��ʾ����ȴ��
        if (_image.fillAmount <= 0)
        {
            _image.fillAmount = 1;
        }
    }
}
