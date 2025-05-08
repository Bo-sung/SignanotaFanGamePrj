using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Movement speed of the player")]
    [SerializeField]
    float m_speed = 5f;
    [SerializeField]
    Vector2 m_inputVec;

    Rigidbody2D m_rb;
    SpriteRenderer m_spr;
    Animator m_animator;

    public Vector2 InputVec => m_inputVec;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_spr = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
    }

    private void OnMove(InputValue value)
    {
        m_inputVec = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector2 moveDirection = m_inputVec.normalized;

        m_rb.MovePosition(m_rb.position + moveDirection * m_speed * Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        m_animator.SetFloat("Speed", m_inputVec.magnitude);

        if (m_inputVec.x != 0 )
        {
            m_spr.flipX = m_inputVec.x < 0;
        }
    }
}
