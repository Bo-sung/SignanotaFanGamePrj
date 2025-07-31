using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData weaponData;

    private Player m_player;
    private float m_timer = 0f;

    // Current stats, can be modified by level-ups and other effects
    private float m_currentDamage;
    private int m_currentCount;
    private float m_currentAttackSpeed;
    private int m_currentPenetration;
    private float m_currentProjectileSpeed;

    private void Awake()
    {
        m_player = GetComponentInParent<Player>();
        if (m_player == null)
        { 
            Debug.LogError("Player component not found in parent game objects.");
        }
    }

    private void Start()
    {
        if (weaponData != null)
        {
            Init();
        }
        else
        {
            Debug.LogError("WeaponData is not assigned to the weapon.");
        }
    }

    private void Update()
    {
        if (weaponData == null) return;

        WeaponLogic();
    }

    public void Init()
    {
        // Initialize current stats from the ScriptableObject
        m_currentDamage = weaponData.damage;
        m_currentCount = weaponData.count;
        m_currentAttackSpeed = weaponData.attackSpeed;
        m_currentPenetration = weaponData.penetration;
        m_currentProjectileSpeed = weaponData.projectileSpeed;

        // Specific initialization logic based on weapon ID
        switch (weaponData.weaponId)
        {
            case 0: // Example: Rotating weapon
                Batch();
                break;
            case 1: // Example: Ranged weapon
                break;
            default:
                break;
        }
    }

    public void LevelUp(float damage, int count, float attackSpeed, int penetration)
    {
        // Apply level-up bonuses
        this.m_currentDamage += damage;
        this.m_currentCount += count;
        this.m_currentAttackSpeed -= attackSpeed; // Assuming lower is better
        this.m_currentPenetration += penetration;

        // Re-apply changes, for example, for rotating weapons
        if (weaponData.weaponId == 0)
        {
            Batch();
        }
    }

    private void WeaponLogic()
    {
        switch (weaponData.weaponId)
        {
            case 0: // Rotating weapon logic
                transform.Rotate(Vector3.back * -m_currentAttackSpeed * Time.deltaTime);
                break;
            default: // Ranged weapon logic
                m_timer += Time.deltaTime;
                if (m_timer > m_currentAttackSpeed)
                {
                    m_timer = 0f;
                    Fire();
                }
                break;
        }
    }

    private void Batch()
    {
        // Clear existing children before creating new ones
        foreach (Transform child in transform)
        {
            GameManager.Instance.poolManager.Release(child.gameObject);
        }

        for (int i = 0; i < m_currentCount; i++)
        {
            if (weaponData.projectilePrefab != null)
            {
                GameObject bulletObj = GameManager.Instance.poolManager.Get(weaponData.projectilePrefab);
                bulletObj.transform.parent = transform;
                bulletObj.transform.localPosition = Vector3.zero;
                bulletObj.transform.localRotation = Quaternion.identity;

                Vector3 rot = Vector3.forward * 360 * i / m_currentCount;
                bulletObj.transform.Rotate(rot);
                bulletObj.transform.Translate(bulletObj.transform.up * 1.5f, Space.World);

                Bullet bulletScript = bulletObj.GetComponent<Bullet>();
                if (bulletScript != null)
                {
                    bulletScript.Init(m_currentDamage, m_currentPenetration, bulletObj.transform.up, m_currentProjectileSpeed);
                }
            }
            else
            {
                Debug.LogError("Projectile prefab is not set in WeaponData.");
            }
        }
    }

    private void Fire()
    {
        if (m_player.Scanner.NearestTarget == null) return;

        Vector3 targetPos = m_player.Scanner.NearestTarget.position;
        Vector3 direction = (targetPos - transform.position).normalized;

        if (weaponData.projectilePrefab != null)
        {
            GameObject projectile = GameManager.Instance.poolManager.Get(weaponData.projectilePrefab);
            projectile.transform.position = transform.position;
            
            Bullet bulletScript = projectile.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.Init(m_currentDamage, m_currentPenetration, direction, m_currentProjectileSpeed);
            }
        }
        else
        {
            Debug.LogError("Projectile prefab is not set in WeaponData.");
        }
    }
}
