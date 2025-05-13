using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField]
    private float m_scanRange = 10f;
    [SerializeField]
    private LayerMask m_targetLayerMask;
    [SerializeField]
    private RaycastHit2D[] m_targets;

    [SerializeField]
    private Transform m_nearestTarget;
    
    public float ScanRange => m_scanRange;
    public RaycastHit2D[] Targets => m_targets;
    public Transform NearestTarget => m_nearestTarget;
    public LayerMask TargetLayerMask => m_targetLayerMask;


    private void FixedUpdate()
    {
        m_targets = Physics2D.CircleCastAll(transform.position, m_scanRange, Vector2.zero, 0f, m_targetLayerMask);
        m_nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float dist = 100;

        foreach(var item in m_targets)
        {
            if (item.transform == null)
                continue;
            float temp = Vector2.Distance(item.transform.position, transform.position);
            if (temp < dist)
            {
                dist = temp;
                result = item.transform;
            }
        }

        return result;
    }
}
