using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    int m_id = 0;
    [SerializeField]
    int m_prefabId = 0;
    [SerializeField]
    float m_damage = 0f;
    [SerializeField]
    int count = 0;
    [SerializeField]
    float m_attackSpeed = 0f;

    private void Start()
    {
        Init();
    }

    public void WeaponLogic()
    {
        switch (m_id)
        {
            case 0:
                transform.Rotate(Vector3.back * m_attackSpeed * Time.deltaTime);
                break;
            default:
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

        if(m_id == 0)
        {
            Batch();
        }
    }

    public void Init()
    {
        switch (m_id)
        {
            case 0:
                m_attackSpeed = -150f;
                Batch();
                break;
            default:
                break;
        }
    }

    private void Batch()
    {
        for(int i = 0; i < count; i++)
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
                bullet = GameManager.Instance.SpawnBullet(data).transform;
                bullet.parent = transform;
            }
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rot = Vector3.forward * 360 * i / count;
            bullet.Rotate(rot);
            bullet.Translate(bullet.up * 1.5f, Space.World);
        }
    }

    private void Update()
    {
        WeaponLogic();
    }

}
