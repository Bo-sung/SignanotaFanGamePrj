using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Info")]
    public int enemyId;
    public string enemyName;
    [TextArea] public string enemyDescription;
    public GameObject enemyPrefab; // The prefab for this enemy

    [Header("Stats")]
    public float maxHealth;
    public float speed;
    public float damage;
    public float attackCooldown;

    [Header("References")]
    public RuntimeAnimatorController animatorController;
    public GameObject dropItemPrefab; // The item to drop on death
}
