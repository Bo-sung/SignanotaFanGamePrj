using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Scanner))]
public class Player : MonoBehaviour
{
    [Header("Slingshot")]
    [Tooltip("The force multiplier for the slingshot launch.")]
    [SerializeField] private float launchForceMultiplier = 5f;
    [Tooltip("The minimum drag distance required to register a launch.")]
    [SerializeField] private float minDragDistance = 0.1f;
    [Tooltip("The velocity magnitude at which the player is considered to be stopped.")]
    [SerializeField] private float stopVelocityThreshold = 0.1f;

    private Rigidbody2D m_rb;
    private SpriteRenderer m_spr;
    private Animator m_animator;
    private Scanner m_scanner;

    private bool m_isDragging = false;
    private Vector2 m_dragStartPosition;

    public Scanner Scanner => m_scanner;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_spr = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
        m_scanner = GetComponent<Scanner>();
    }

    private void Update()
    {
        // Use the new Input System to handle mouse input.
        Mouse mouse = Mouse.current;
        if (mouse == null) return; // Exit if no mouse is present

        // On mouse button press, start dragging.
        if (mouse.leftButton.wasPressedThisFrame)
        {
            m_isDragging = true;
            m_dragStartPosition = GetMouseWorldPosition();
        }
        // On mouse button release, end dragging and launch.
        else if (mouse.leftButton.wasReleasedThisFrame && m_isDragging)
        {
            m_isDragging = false;
            Launch();
        }
    }

    private void FixedUpdate()
    {
        // If not dragging and the player is moving slowly, bring them to a complete stop.
        if (!m_isDragging && m_rb.velocity.magnitude > 0 && m_rb.velocity.magnitude < stopVelocityThreshold)
        {
            m_rb.velocity = Vector2.zero;
            m_rb.angularVelocity = 0f;
        }
    }

    private void LateUpdate()
    {
        // Flip the sprite based on the horizontal velocity.
        if (m_rb.velocity.x != 0)
        {
            m_spr.flipX = m_rb.velocity.x < 0;
        }
    }

    /// <summary>
    /// Calculates the launch vector and applies force to the Rigidbody.
    /// </summary>
    private void Launch()
    {
        Vector2 dragEndPosition = GetMouseWorldPosition();
        Vector2 launchVector = m_dragStartPosition - dragEndPosition; // Direction is from end to start

        // Only launch if the drag distance is significant enough.
        if (launchVector.magnitude > minDragDistance)
        {
            float force = launchVector.magnitude * launchForceMultiplier;
            m_rb.AddForce(launchVector.normalized * force, ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// Gets the current mouse position in world coordinates.
    /// </summary>
    /// <returns>The mouse position in 2D world space.</returns>
    private Vector2 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }
}

