using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    float m_speed = 1f;
    [SerializeField]
    float m_health = 0f;
    [SerializeField]
    float m_MaxHealth = 0f;
    [SerializeField]
    Rigidbody2D m_target;
    [SerializeField]
    RuntimeAnimatorController[] m_animatorController;

    bool isLive = false;
    Rigidbody2D m_rb;
    SpriteRenderer m_spr;
    Animator m_animator;


    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_spr = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
        if (m_target == null)
        {
            var target = GameManager.Instance.Player;
            m_target = target.GetComponent<Rigidbody2D>();
        }
    }

    private void OnEnable()
    {
        if (m_target == null)
        {
            var target = GameManager.Instance.Player;
            m_target = target.GetComponent<Rigidbody2D>();
        }
        m_health = m_MaxHealth;
    }

    public void Init(SpawnData _data)
    {
        m_animator.runtimeAnimatorController = m_animatorController[_data.spriteType];
        m_speed = _data.speed;
        m_MaxHealth = _data.health;
        m_health = _data.health;
        isLive = true;
    }

    private void FixedUpdate()
    {
        if (!isLive)
            return;

        Vector2 targetPos = m_target.position;
        Vector2 moveDir = targetPos - m_rb.position;
        moveDir.Normalize();
        Vector2 moveVector = moveDir * m_speed * Time.fixedDeltaTime;

        m_rb.MovePosition(m_rb.position + moveVector);

        m_spr.flipX = moveDir.x < 0;

        if (m_health <= 0)
        {
            isLive = false;
        }
    }
}
