using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 游戏中的UI
/// </summary>
public class UI_InGame : MonoBehaviour
{
    //玩家状态
    [SerializeField] private PlayerStats playerStats;
    //创建一个滑动条
    [SerializeField] private Slider slider;
    
    //冲刺图标
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
    /// 更新UI的健康值
    /// </summary>
    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }

    /// <summary>
    /// 设置冷却
    /// </summary>
    /// <param name="_image"></param>
    private void SetCooldownOf(Image _image)
    {
        //范围为 0-1，0 为可以使用，1 为显示在冷却。
        if (_image.fillAmount <= 0)
        {
            _image.fillAmount = 1;
        }
    }
}
