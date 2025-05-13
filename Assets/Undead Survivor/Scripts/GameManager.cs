using UnityEngine;


public enum PrefabsType
{
    Enemy = 0,
    Melee = 1,
    Bullet_1 = 2,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField]
    Player m_player;
    public Player Player => m_player;
    [SerializeField]
    PoolManager m_poolManager;
    private PoolManager PoolManager => m_poolManager;

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

   // bool m_isFreeze = false;

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

    //public void SetFreezeEnemy(bool _isFreeze)
    //{
    //    m_isFreeze = _isFreeze;
    //    var enemyPool = m_poolManager.GetPool(PrefabsType.Enemy);
    //
    //    foreach (var item in enemyPool)
    //    {
    //        if (item.activeSelf)
    //        {
    //            var enemy = item.GetComponent<Enemy>();
    //            if (enemy != null)
    //            {
    //                enemy.freeze = _isFreeze;
    //            }
    //        }
    //    }
    //}

    public GameObject SpawnEnemy(SpawnData_Enemy _spawnData)
    {
        var temp =  PoolManager.Get(PrefabsType.Enemy);
        if (temp == null)
            return null;
        var enemy = temp.GetComponent<Enemy>();
        if (enemy == null)
        {
            Debug.LogError("Enemy component not found on the prefab.");
            Destroy(temp);
            return null;
        }
        enemy.Init(_spawnData);
        //enemy.freeze = m_isFreeze;
        return temp;
    }

    public GameObject SpawnBullet(PrefabsType type, SpawnData_Bullet _bulletData)
    {
        var temp = PoolManager.Get(type);
        if (temp == null)
            return null;
        temp.GetComponent<Bullet>()?.Init(_bulletData);
        return temp;
    }
}
