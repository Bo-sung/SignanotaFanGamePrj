using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    [Header("Weapon Setup")]
    [Tooltip("The container object for holding weapon scripts.")]
    [SerializeField] private Transform weaponContainer;

    private List<Weapon> m_equippedWeapons;

    private void Awake()
    {
        m_equippedWeapons = new List<Weapon>();
        if (weaponContainer == null)
        {
            // If no container is assigned, create one dynamically.
            weaponContainer = new GameObject("WeaponContainer").transform;
            weaponContainer.parent = this.transform;
            weaponContainer.localPosition = Vector3.zero;
        }
    }

    public void AddWeapon(WeaponData weaponData)
    {
        // Prevent equipping the same weapon twice.
        foreach (Weapon w in m_equippedWeapons)
        {
            if (w.weaponData == weaponData)
            {
                Debug.LogWarning($"Weapon {weaponData.weaponName} is already equipped.");
                return;
            }
        }

        // Create a new GameObject for the weapon logic.
        GameObject weaponObj = new GameObject(weaponData.weaponName);
        weaponObj.transform.parent = weaponContainer;
        weaponObj.transform.localPosition = Vector3.zero;

        // Add the Weapon component and initialize it.
        Weapon newWeapon = weaponObj.AddComponent<Weapon>();
        newWeapon.weaponData = weaponData;
        
        m_equippedWeapons.Add(newWeapon);
        Debug.Log($"Equipped weapon: {weaponData.weaponName}");
    }

    public void LevelUpWeapon(WeaponData weaponToUpgrade, float damage, int count, float attackSpeed, int penetration)
    {
        foreach (Weapon weapon in m_equippedWeapons)
        {
            if (weapon.weaponData == weaponToUpgrade)
            {
                weapon.LevelUp(damage, count, attackSpeed, penetration);
                Debug.Log($"Leveled up weapon: {weaponToUpgrade.weaponName}");
                return;
            }
        }
        Debug.LogWarning($"Attempted to level up a weapon that is not equipped: {weaponToUpgrade.weaponName}");
    }
}
