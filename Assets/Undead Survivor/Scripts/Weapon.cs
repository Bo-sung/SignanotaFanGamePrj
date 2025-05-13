using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    int m_id = 0;
    [SerializeField]
    PrefabsType m_prefabId = PrefabsType.Melee;
    [SerializeField]
    float m_damage = 0f;
    [SerializeField]
    int count = 0;
    [SerializeField]
    float m_attackSpeed = 0f;

    private Player m_player;

    private float m_timer = 0f;

    private void Awake()
    {
        m_player = GetComponentInParent<Player>();
        if (m_player == null)
        {
            Debug.LogError("Player not found in parent.");
        }
    }

    private void Start()
    {
        Init();
    }

    public void WeaponLogic()
    {
        switch (m_id)
        {
            case 0:
                transform.Rotate(Vector3.back * -m_attackSpeed * Time.deltaTime);
                break;
            default:
                m_timer += Time.deltaTime;

                if (m_timer > m_attackSpeed)
                {
                    m_timer = 0f;
                    Fire();
                }
                break;
        }
    }

    [ContextMenu("TestLVUp")]
    public void TestLVUp()
    {
        LevelUp(1f, 1);
    }

    public void LevelUp(float damage, int count)
    {
        this.m_damage += damage;
        this.count += count;

        if (m_id == 0)
        {
            Batch();
        }
    }

    public void Init()
    {
        switch (m_id)
        {
            case 0:
                m_attackSpeed = 150f;
                Batch();
                break;

            case 1:
                m_attackSpeed = 0.5f;
                break;
            default:
                break;
        }
    }

    private void Batch()
    {
        for (int i = 0; i < count; i++)
        {
            SpawnData_Bullet data = new SpawnData_Bullet();
            data.damage = m_damage;
            data.per = -1;  // -1은 무한대
            Transform bullet;
            if (i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            else
            {
                bullet = GameManager.Instance.SpawnBullet(m_prefabId, data).transform;
                bullet.parent = transform;
            }
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rot = Vector3.forward * 360 * i / count;
            bullet.Rotate(rot);
            bullet.Translate(bullet.up * 1.5f, Space.World);
        }
    }

    void Fire()
    {
        if (m_id == 0)
        {
            return;
        }
        if (m_player == null)
        {
            Debug.LogError("Player not found.");
            return;
        }
        var temp = m_player.Scanner.NearestTarget;
        if (temp == null)
        {
            Debug.LogError("Target not found.");
            return;
        }

        Vector3 targetPos = temp.position;

        Vector3 direction = (targetPos - transform.position).normalized;



        SpawnData_Bullet data = new SpawnData_Bullet();
        data.damage = m_damage;
        data.per = count;
        data.direction = direction;
        var instance = GameManager.Instance.SpawnBullet(m_prefabId, data);
        if (instance == null)
        {
            Debug.LogError("Bullet instance is null.");
            return;
        }

        instance.transform.position = transform.position;
        instance.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
    }

    private void Update()
    {
        WeaponLogic();
    }

}
