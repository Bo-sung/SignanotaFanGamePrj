using UnityEngine;
using UnityEngine.UI;
using TMPro; // Make sure you have TextMeshPro imported

public class RewardCard : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button selectButton;

    private UpgradeData m_upgradeData;
    private LevelUpUI m_levelUpUI;

    public void Setup(UpgradeData upgradeData, LevelUpUI levelUpUI)
    {
        m_upgradeData = upgradeData;
        m_levelUpUI = levelUpUI;

        if (upgradeData != null && upgradeData.weaponData != null)
        {
            iconImage.sprite = upgradeData.weaponData.weaponIcon;
            nameText.text = upgradeData.weaponData.weaponName;
            descriptionText.text = upgradeData.upgradeDescription;
        }

        selectButton.onClick.AddListener(HandleSelection);
    }

    private void HandleSelection()
    {
        if (m_upgradeData != null && m_levelUpUI != null)
        {
            m_levelUpUI.OnRewardSelected(m_upgradeData);
        }
    }

    private void OnDestroy()
    {
        selectButton.onClick.RemoveListener(HandleSelection);
    }
}
