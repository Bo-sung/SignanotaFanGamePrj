using UnityEngine;

// CreateAssetMenu attribute allows us to create instances of this ScriptableObject from the Unity Editor.
[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Data/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Info")]
    public int weaponId;
    public string weaponName;
    [TextArea] public string weaponDescription;
    public Sprite weaponIcon;

    [Header("Stats")]
    public float damage;
    public int count; // Number of projectiles or attacks
    public float attackSpeed;
    public int penetration;
    public float projectileSpeed;

    [Header("References")]
    public GameObject projectilePrefab; // The prefab for the bullet/projectile
}
