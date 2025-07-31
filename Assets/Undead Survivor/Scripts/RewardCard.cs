using UnityEngine;
using UnityEngine.UI;
using TMPro; // Make sure you have TextMeshPro imported

// This class represents a single card in the level up selection screen.
public class RewardCard : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button selectButton;

    private WeaponData m_weaponData;
    private LevelUpUI m_levelUpUI;

    public void Setup(WeaponData weaponData, LevelUpUI levelUpUI)
    {
        m_weaponData = weaponData;
        m_levelUpUI = levelUpUI;

        // Populate the card's UI elements with data from the ScriptableObject
        if (weaponData != null)
        {
            iconImage.sprite = weaponData.weaponIcon;
            nameText.text = weaponData.weaponName;
            descriptionText.text = weaponData.weaponDescription;
        }

        // Add a listener to the button to handle the selection
        selectButton.onClick.AddListener(HandleSelection);
    }

    private void HandleSelection()
    {
        if (m_weaponData != null && m_levelUpUI != null)
        {
            m_levelUpUI.OnRewardSelected(m_weaponData);
        }
    }

    // Clean up the listener when the card is destroyed
    private void OnDestroy()
    {
        selectButton.onClick.RemoveListener(HandleSelection);
    }
}
