using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class will be responsible for managing the level up selection screen.
public class LevelUpUI : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The main panel for the level up screen.")]
    [SerializeField] private GameObject levelUpPanel;
    [Tooltip("The container for the reward option cards.")]
    [SerializeField] private Transform optionsContainer;
    [Tooltip("The prefab for a single reward option card.")]
    [SerializeField] private GameObject rewardCardPrefab;

    // This could be a list of ScriptableObjects representing possible upgrades.
    // For now, we'll just pass WeaponData directly.

    private void Start()
    {
        // Start with the panel hidden.
        levelUpPanel.SetActive(false);
    }

    public void ShowLevelUpOptions(List<WeaponData> rewardOptions)
    {
        // Pause the game
        Time.timeScale = 0f;

        // Clear any previous options
        foreach (Transform child in optionsContainer)
        {
            Destroy(child.gameObject);
        }

        // Show the panel
        levelUpPanel.SetActive(true);

        // Create a card for each reward option
        foreach (WeaponData reward in rewardOptions)
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
        // Resume the game
        Time.timeScale = 1f;
    }

    public void OnRewardSelected(WeaponData selectedWeapon)
    {
        // Pass the choice to the EquipmentManager
        // This is a simplified example. You might need to differentiate between a new weapon and an upgrade.
        GameManager.Instance.equipmentManager.AddWeapon(selectedWeapon);

        // Hide the screen and resume the game
        HideLevelUpScreen();
    }
}
