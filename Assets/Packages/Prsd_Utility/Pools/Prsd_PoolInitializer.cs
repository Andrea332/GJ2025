using UnityEngine;


/// <summary>
/// Initializes all pools in the field on Awake.
/// </summary>
public class Prsd_PoolInitializer : MonoBehaviour
{
    [SerializeField] Prsd_PoolObject[] pools;

    void Awake()
    {
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i].Init();
        }
    }

    void OnDestroy()
    {
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i].Clear();
        }
    }
}
