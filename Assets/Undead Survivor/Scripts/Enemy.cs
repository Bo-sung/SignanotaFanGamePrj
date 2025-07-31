using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;

    private float m_currentHealth;
    private Rigidbody2D m_rb;
    private SpriteRenderer m_spr;
    private Animator m_animator;
    private Transform m_target;

    private bool m_isLive = false;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_spr = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // Find the player when the enemy is enabled
        if (GameManager.Instance != null && GameManager.Instance.Player != null)
        {
            m_target = GameManager.Instance.Player.transform;
        }
        else
        {
            Debug.LogError("Player not found!");
            m_isLive = false;
            return;
        }

        if (enemyData != null)
        {
            Init();
        }
        else
        {
            Debug.LogError("EnemyData is not assigned!");
            m_isLive = false;
        }
    }

    public void Init()
    {
        m_currentHealth = enemyData.maxHealth;
        m_animator.runtimeAnimatorController = enemyData.animatorController;
        m_isLive = true;
    }

    private void FixedUpdate()
    {
        if (!m_isLive || m_target == null) return;

        // Movement towards the player
        Vector2 direction = (m_target.position - transform.position).normalized;
        m_rb.MovePosition(m_rb.position + direction * enemyData.speed * Time.fixedDeltaTime);

        // Flip sprite based on movement direction
        m_spr.flipX = direction.x < 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Collision with a bullet
        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
            }
            // Optionally, disable the bullet if it doesn't have penetration
            if (bullet.per <= 0)
            {
                collision.gameObject.SetActive(false);
            }
        }
        // Collision with the player
        else if (collision.CompareTag("Player"))
        { 
            // Deal damage to the player, knockback, etc.
            // This part will be implemented later.
        }
    }

    public void TakeDamage(float damage)
    {
        if (!m_isLive) return;

        m_currentHealth -= damage;

        if (m_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        m_isLive = false;

        // Drop item if specified, using the pool manager
        if (enemyData.dropItemPrefab != null)
        {
            GameObject gem = GameManager.Instance.poolManager.Get(enemyData.dropItemPrefab);
            gem.transform.position = transform.position;
        }

        // Release the enemy back to the pool
        GameManager.Instance.poolManager.Release(gameObject);
    }
}
