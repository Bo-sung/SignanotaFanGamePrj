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
    [SerializeField] private float m_MaxGameTime = 20f;
    public float MaxGameTime => m_MaxGameTime;

    [Header("Level & Experience")]
    [SerializeField] private int m_Level = 1;
    public int Level => m_Level;
    [SerializeField] private int m_Experience = 0;
    [SerializeField] private int[] m_nextExp; // Experience required for next level

    // Reference to the PoolManager
    public PoolManager poolManager { get; private set; }
    // Reference to the EquipmentManager
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

            // Initialize experience table
            m_nextExp = new int[100]; // Max level 100
            for (int i = 0; i < m_nextExp.Length; i++)
            {
                m_nextExp[i] = Mathf.FloorToInt(10 * Mathf.Pow(1.1f, i));
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        m_GameTime += Time.deltaTime;
        if (m_GameTime > m_MaxGameTime)
        {
            m_GameTime = m_MaxGameTime;
        }
    }

    public void AddExperience(int amount)
    {
        m_Experience += amount;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (m_Level - 1 >= m_nextExp.Length) return; // Max level reached

        if (m_Experience >= m_nextExp[m_Level - 1])
        {
            m_Experience -= m_nextExp[m_Level - 1];
            m_Level++;
            Debug.Log($"Level Up! New Level: {m_Level}");
            // Here you would typically pause the game and show the level up UI.
            // For now, we just log it.

            // Recursive call to handle multiple level ups at once
            CheckLevelUp();
        }
    }
}