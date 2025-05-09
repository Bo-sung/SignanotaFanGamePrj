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
            Transform bullet = GameManager.Instance.PoolManager.Get(m_prefabId).transform;
            bullet.parent = transform;
            Vector3 rot = Vector3.forward * 360 * i / count;
            bullet.Rotate(rot);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(m_damage, -1); // -1은 무한대
        }
    }

    private void Update()
    {
        WeaponLogic();
    }

}
