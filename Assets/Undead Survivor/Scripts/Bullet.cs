using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private SpawnData_Bullet m_data;

    public float Damage => m_data.damage;
    public int Per => m_data.per;

    public void Init(SpawnData_Bullet _data)
    {
        this.m_data = _data;
    }
}

[System.Serializable]
public class SpawnData_Bullet
{
    public float damage;
    public int per;
}

