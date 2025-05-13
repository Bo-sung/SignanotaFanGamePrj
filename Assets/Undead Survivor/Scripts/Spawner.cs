using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] SpawnPoints;
    [SerializeField]
    private SpawnData_Enemy[] m_SpawnData;

    float m_Timer = 0;
    int m_level = 0;

    private void Awake()
    {
        SpawnPoints = GetComponentsInChildren<Transform>();
    }
    void Update()
    {
        m_Timer += Time.deltaTime;
        m_level = GameManager.Instance.Level;

        if (m_Timer > m_SpawnData[m_level].spawnTime)
        {
            m_Timer = 0;
            Spawn();
        }
    }

    private void Spawn()
    {
        Spawn(Random.Range(0, m_SpawnData.Length - 1));
    }

    private void Spawn(int type)
    {
        var enemy = GameManager.Instance.SpawnEnemy(m_SpawnData[type]);
        if (enemy == null)
            return;

        // 랜덤 스폰 포인트
        int randomIndex = UnityEngine.Random.Range(1, SpawnPoints.Length);
        // 스폰 포인트의 위치를 가져옴
        Vector3 spawnPosition = SpawnPoints[randomIndex].position;
        // 스폰 포인트의 회전값을 가져옴
        Quaternion spawnRotation = SpawnPoints[randomIndex].rotation;
        // 적을 스폰 포인트에 생성
        enemy.transform.position = spawnPosition;
        enemy.transform.rotation = spawnRotation;
        // 적을 활성화
        enemy.SetActive(true);
    }
}

[System.Serializable]
public class SpawnData_Enemy
{
    public int spriteType;
    public float spawnTime;
    public int health;
    public float speed;
}
