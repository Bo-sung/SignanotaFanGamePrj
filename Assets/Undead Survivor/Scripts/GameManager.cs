using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player")]
    [SerializeField] private Player m_player;
    public Player Player => m_player;

    [Header("Game State")]
    [SerializeField] private float m_GameTime = 0;
    public float GameTime => m_GameTime;
    [SerializeField] private float m_MaxGameTime = 1200f; // 20 minutes

    [Header("Level & Experience")]
    [SerializeField] private int m_Level = 1;
    public int Level => m_Level;
    [SerializeField] private int m_Experience = 0;
    [SerializeField] private int[] m_nextExp;

    [Header("Upgrades & UI")]
    [Tooltip("A list of all possible weapon upgrades in the game.")]
    [SerializeField] private List<WeaponData> allWeapons;
    [Tooltip("Reference to the Level Up UI controller.")]
    [SerializeField] private LevelUpUI levelUpUI;

    public PoolManager poolManager { get; private set; }
    public EquipmentManager equipmentManager { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            poolManager = GetComponent<PoolManager>();
            if (poolManager == null) poolManager = FindObjectOfType<PoolManager>();
            if (poolManager == null) Debug.LogError("PoolManager not found in the scene.");

            if (m_player != null)
            {
                equipmentManager = m_player.GetComponent<EquipmentManager>();
                if (equipmentManager == null) Debug.LogError("EquipmentManager not found on the Player.");
            }
            else
            {
                Debug.LogError("Player is not assigned in the GameManager.");
            }

            if (levelUpUI == null)
            {
                levelUpUI = FindObjectOfType<LevelUpUI>();
                if (levelUpUI == null) Debug.LogError("LevelUpUI not found in the scene.");
            }

            InitializeExperienceTable();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeExperienceTable()
    {
        m_nextExp = new int[100];
        m_nextExp[0] = 10;
        for (int i = 1; i < m_nextExp.Length; i++)
        {
            m_nextExp[i] = Mathf.FloorToInt(m_nextExp[i - 1] * 1.2f);
        }
    }

    private void Update()
    {
        m_GameTime += Time.deltaTime;
        if (m_GameTime > m_MaxGameTime)
        {
            m_GameTime = m_MaxGameTime;
            // Game Over or Win condition
        }
    }

    public void AddExperience(int amount)
    {
        m_Experience += amount;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (m_Level - 1 >= m_nextExp.Length) return; // Max level

        if (m_Experience >= m_nextExp[m_Level - 1])
        {
            m_Experience -= m_nextExp[m_Level - 1];
            m_Level++;
            Debug.Log($"Level Up! New Level: {m_Level}");

            // Show level up options
            levelUpUI.ShowLevelUpOptions(GetLevelUpRewards());

            CheckLevelUp(); // Handle multiple level ups
        }
    }

    private List<UpgradeData> GetLevelUpRewards()
    {
        List<UpgradeData> rewards = new List<UpgradeData>();
        List<WeaponData> availableNewWeapons = allWeapons.Except(equipmentManager.GetEquippedWeapons().Select(w => w.weaponData)).ToList();
        List<Weapon> equippedWeapons = equipmentManager.GetEquippedWeapons();

        // Create a combined list of potential upgrades
        List<UpgradeData> potentialUpgrades = new List<UpgradeData>();

        // Add new weapons to the list
        foreach (var weapon in availableNewWeapons)
        {
            potentialUpgrades.Add(new UpgradeData { type = UpgradeData.UpgradeType.NewWeapon, weaponData = weapon, upgradeDescription = "New! " + weapon.weaponDescription });
        }

        // Add upgrades for existing weapons
        foreach (var weapon in equippedWeapons)
        {
            // This is a placeholder for a more complex upgrade system.
            // A real game might have specific upgrade paths for each weapon.
            potentialUpgrades.Add(new UpgradeData { type = UpgradeData.UpgradeType.WeaponUpgrade, weaponData = weapon.weaponData, upgradeDescription = "Upgrade! Damage +1" });
        }

        // Randomly pick 3 unique rewards
        System.Random rand = new System.Random();
        while (rewards.Count < 3 && potentialUpgrades.Count > 0)
        {
            int index = rand.Next(potentialUpgrades.Count);
            rewards.Add(potentialUpgrades[index]);
            potentialUpgrades.RemoveAt(index);
        }

        return rewards;
    }
}