using Unity.VisualScripting;
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

    Vector2 m_firstMousePos;
    Vector2 m_lastMousePos;

    Rigidbody2D m_rb;
    SpriteRenderer m_spr;
    Animator m_animator;
    Mouse m_mouse;

    bool m_dragging = false;
    bool m_isMoving = false;

    public Vector2 InputVec => m_inputVec;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_spr = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();

        m_mouse = Mouse.current;
    }

    private void OnMove(InputValue value)
    {
        m_inputVec = value.Get<Vector2>();
    }

    private void OnMouseDrag()
    {
        m_lastMousePos = Mouse.current.position.ReadValue();
    }

    private void OnMouseUp()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(m_lastMousePos);
        Vector2 direction = (worldPos - m_rb.position);
        float magnitude = direction.magnitude;
        direction.Normalize();
        direction *= -1; // invert direction
        m_rb.AddForce(direction * m_speed, ForceMode2D.Impulse);
    }

    //private void Update()
    //{
    //    if (m_mouse.leftButton.isPressed && m_dragging == false)
    //        OnDragStart();
    //    else if (m_mouse.leftButton.isPressed == false && m_dragging == true)
    //        OnDragEnd();
    //    
    //    if (!m_dragging && m_isMoving)
    //    {
    //        if(m_rb.linearVelocity.magnitude <= 0.5)
    //        {
    //            m_rb.linearVelocity = Vector2.zero;
    //            m_rb.angularVelocity = 0f;
    //            //GameManager.Instance.SetFreezeEnemy(false);
    //        }
    //    }
    //}

    private void OnDragStart()
    {
        m_dragging = true;
        //GameManager.Instance.SetFreezeEnemy(true);
    }

    private void OnDragEnd()
    {
        m_dragging = false;
        m_lastMousePos = Mouse.current.position.ReadValue();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(m_lastMousePos);
        Vector2 direction = (worldPos - m_rb.position);
        float magnitude = direction.magnitude;
        direction.Normalize();
        direction *= -1; // invert direction
        m_rb.AddForce(direction * m_speed * magnitude, ForceMode2D.Impulse);
        m_isMoving = true;
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
