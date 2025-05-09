using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float m_damage = 10f;
    [SerializeField]
    private int m_per;

    public float Damage => m_damage;
    public int Per => m_per;

    public void Init(float damage, int per)
    {
        this.m_damage = damage;
        this.m_per = per;
    }
}
