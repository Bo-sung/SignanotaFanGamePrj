using UnityEngine;

public class ExperienceGem : MonoBehaviour
{
    [Header("Experience")]
    [Tooltip("The amount of experience this gem provides.")]
    [SerializeField] private int experienceValue = 1;

    [Header("Magnet Effect")]
    [Tooltip("The distance at which the gem starts moving towards the player.")]
    [SerializeField] private float magnetDistance = 3f;
    [Tooltip("The speed at which the gem moves towards the player.")]
    [SerializeField] private float magnetSpeed = 5f;

    private Transform m_playerTransform;
    private bool m_isFollowing = false;

    private void OnEnable()
    {
        // Reset state when enabled from the pool
        m_isFollowing = false;
        if (GameManager.Instance != null && GameManager.Instance.Player != null)
        {
            m_playerTransform = GameManager.Instance.Player.transform;
        }
    }

    private void Update()
    {
        if (m_playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, m_playerTransform.position);

        // Start following if the player is close enough
        if (distanceToPlayer < magnetDistance)
        {
            m_isFollowing = true;
        }

        // Move towards the player if following
        if (m_isFollowing)
        {
            transform.position = Vector2.MoveTowards(transform.position, m_playerTransform.position, magnetSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the gem collides with the player, grant experience and return to the pool.
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.AddExperience(experienceValue);
            GameManager.Instance.poolManager.Release(gameObject);
        }
    }
}
