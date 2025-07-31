using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// A simple class to hold information about a potential upgrade.
public class UpgradeData
{
    public enum UpgradeType { NewWeapon, WeaponUpgrade }

    public UpgradeType type;
    public WeaponData weaponData; // The weapon to add or upgrade
    public string upgradeDescription; // e.g., "Damage +10%", "Increases projectile count"
}

public class LevelUpUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject levelUpPanel;
    [SerializeField] private Transform optionsContainer;
    [SerializeField] private GameObject rewardCardPrefab;

    private void Start()
    {
        levelUpPanel.SetActive(false);
    }

    public void ShowLevelUpOptions(List<UpgradeData> rewardOptions)
    {
        Time.timeScale = 0f;

        foreach (Transform child in optionsContainer)
        {
            Destroy(child.gameObject);
        }

        levelUpPanel.SetActive(true);

        foreach (UpgradeData reward in rewardOptions)
        {
            GameObject cardObj = Instantiate(rewardCardPrefab, optionsContainer);
            RewardCard card = cardObj.GetComponent<RewardCard>();
            if (card != null)
            {
                card.Setup(reward, this);
            }
        }
    }

    public void HideLevelUpScreen()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnRewardSelected(UpgradeData selectedUpgrade)
    {
        if (selectedUpgrade.type == UpgradeData.UpgradeType.NewWeapon)
        {
            GameManager.Instance.equipmentManager.AddWeapon(selectedUpgrade.weaponData);
        }
        else if (selectedUpgrade.type == UpgradeData.UpgradeType.WeaponUpgrade)
        {
            // The actual stat increase should be defined in the UpgradeData
            // For now, we'll use placeholder values.
            GameManager.Instance.equipmentManager.LevelUpWeapon(selectedUpgrade.weaponData, 1, 1, 0.1f, 1);
        }

        HideLevelUpScreen();
    }
}
