using UnityEngine;
using System.Collections.Generic;
public class PoolManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] m_prefabs;

    [SerializeField]
    List<GameObject>[] m_pools;

    private void Awake()
    {
        m_pools = new List<GameObject>[m_prefabs.Length];

        for (int index = 0; index < m_pools.Length; index++)
        {
            m_pools[index] = new List<GameObject>();
        }
    }

    public List<GameObject> GetPool(PrefabsType type)
    {
        return GetPool((int)type);
    }

    public List<GameObject> GetPool(int index)
    {
        // 인덱스 초과 방지
        if (m_prefabs.Length <= index)
            return null;
        return m_pools[index];
    }

    public GameObject Get(PrefabsType type)
    {
        return Get((int)type);
    }

    private GameObject Get(int index)
    {
        GameObject select = null;
        // 인덱스 초과 방지
        if (m_prefabs.Length <= index)
            return null;

        // 비활성화된 오브젝트가 있는지 확인
        foreach (GameObject obj in m_pools[index])
        {
            if (!obj.activeSelf)
            {
                select = obj;
                select.SetActive(true);

                break;
            }
        }

        // 풀에 비활성화된 오브젝트가 없을 경우 새로 생성
        if (select == null)
        {
            select = Instantiate(m_prefabs[index], transform);
            m_pools[index].Add(select);
        }

        return select;
    }
}
