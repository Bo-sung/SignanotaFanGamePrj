using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField]
    Player m_player;
    public Player Player => m_player;
    [SerializeField]
    PoolManager m_poolManager;
    public PoolManager PoolManager => m_poolManager;

    [SerializeField]
    private float m_GameTime = 0;
    public float GameTime => m_GameTime;

    [SerializeField]
    private float m_MaxGameTime = 20f;
    public float MaxGameTime => m_MaxGameTime;

    [SerializeField]
    private float m_SpawnTime = 1f;
    public float SpawnTime => m_SpawnTime;

    [SerializeField]
    private int m_Level = 0;
    public int Level => m_Level;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
        m_Level = Mathf.FloorToInt(m_GameTime / 10f);
    }
}
