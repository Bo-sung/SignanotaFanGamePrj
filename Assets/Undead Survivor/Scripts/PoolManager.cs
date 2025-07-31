using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [Tooltip("The prefabs that will be pooled.")]
    [SerializeField] private GameObject[] prefabs;

    private Dictionary<GameObject, Queue<GameObject>> m_poolDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePools();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePools()
    {
        m_poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

        foreach (GameObject prefab in prefabs)
        {
            m_poolDictionary.Add(prefab, new Queue<GameObject>());
        }
    }

    public GameObject Get(GameObject prefab)
    {
        if (!m_poolDictionary.ContainsKey(prefab))
        {
            Debug.LogWarning($"Pool for prefab {prefab.name} does not exist.");
            return null;
        }

        Queue<GameObject> poolQueue = m_poolDictionary[prefab];

        if (poolQueue.Count > 0)
        {
            GameObject obj = poolQueue.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject newObj = Instantiate(prefab);
            return newObj;
        }
    }

    public void Release(GameObject obj)
    {
        GameObject prefab = obj.GetComponent<Poolable>()?.prefab;
        if (prefab == null)
        {
            Debug.LogWarning($"Object {obj.name} does not have a Poolable component with a prefab reference. Destroying it instead.");
            Destroy(obj);
            return;
        }

        if (!m_poolDictionary.ContainsKey(prefab))
        { 
            Debug.LogWarning($"Pool for prefab {prefab.name} does not exist. Creating a new one.");
            m_poolDictionary.Add(prefab, new Queue<GameObject>());
        }

        obj.SetActive(false);
        m_poolDictionary[prefab].Enqueue(obj);
    }
}

// Add this component to any prefab that you want to be poolable.
public class Poolable : MonoBehaviour
{
    public GameObject prefab;
}
