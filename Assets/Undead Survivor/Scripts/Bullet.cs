using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private SpawnData_Bullet m_data;

    public float Damage => m_data.damage;
    public int Per => m_data.per;

    Rigidbody2D m_rb;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    public void Init(SpawnData_Bullet _data)
    {
        this.m_data = _data;
        if(m_data.per > -1)
        {
            m_rb.linearVelocity = m_data.direction;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || m_data.per == -1)
            return;

        m_data.per--;
        if (m_data.per == -1)
        {
            m_rb.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}

[System.Serializable]
public class SpawnData_Bullet
{
    public float damage;
    public int per;
    public Vector3 direction;
}

