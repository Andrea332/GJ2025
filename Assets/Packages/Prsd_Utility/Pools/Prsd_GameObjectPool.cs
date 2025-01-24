using System;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable, InlineProperty]
public class Prsd_GameObjectPool
{
    [Tooltip("GameObject to be instantiated by the pool.")]
    [BoxGroup("gopool", false), HideLabel]
    [SerializeField] GameObject prefab;

    [Tooltip("Max number of objects the pool can contain.")]
    [HorizontalGroup("gopool/1"), LabelWidth(60f)]
    [SerializeField] int capacity = 100;

    [Tooltip("Number of objects that get preventively instantiated when the pool is initialized.")]
    [HorizontalGroup("gopool/1"), LabelWidth(60f)]
    [SerializeField] int backlog = 0;

    [Tooltip("If pool objects have to be placed in a manually defined scene.")]
    [HorizontalGroup("gopool/2"), ToggleLeft]
    [SerializeField] bool useCustomScene;

    [Tooltip("Name of the scene containing pool objects.")]
    [HorizontalGroup("gopool/2"), ShowIf(nameof(useCustomScene)), HideLabel]
    [SerializeField] string sceneName;

    Prsd_PoolObject pool;

    public Prsd_PoolObject Pool
    {
        get
        {
            if (!pool)
            {
                Init();
            }
            return pool;
        }
    }

    public Prsd_GameObjectPool(GameObject prefab, int capacity, int backlog, bool useCustomScene, string sceneName = "")
    {
        this.prefab = prefab;
        this.capacity = capacity;
        this.backlog = backlog;
        this.useCustomScene = useCustomScene;
        this.sceneName = sceneName;
        Init();
    }

    public void Init()
    {
        if (!prefab)
        {
            Debug.LogError("Prefab for GameObjectPool is null.");
            return;
        }
        pool = ScriptableObject.CreateInstance<Prsd_PoolObject>();
        pool.prefab = prefab;
        pool.capacity = capacity;
        pool.backlog = backlog;
        pool.useCustomScene = useCustomScene;
        pool.sceneName = sceneName;
        pool.Init();
    }
}
