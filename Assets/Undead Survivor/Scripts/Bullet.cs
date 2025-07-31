using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per; // 관통 횟수

    private Rigidbody2D m_rb;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir, float speed)
    {
        this.damage = damage;
        this.per = per;

        if (per > -1)
        {
            m_rb.velocity = dir * speed;
        }
    }

    public void SetDamage(float newDamage)
    {
        this.damage = newDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -100) // -100 is a magic number for infinite penetration
            return;

        per--;
        if (per < 0)
        {
            m_rb.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        // Reset velocity when disabled to avoid unexpected movement when reused from pool
        if (m_rb != null)
        {
            m_rb.velocity = Vector2.zero;
        }
    }
}

