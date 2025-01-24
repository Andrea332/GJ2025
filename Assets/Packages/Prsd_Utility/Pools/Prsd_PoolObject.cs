using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

    public abstract class Prsd_Pooler : ScriptableObject
    {
        public abstract GameObject Get();
        public abstract void Recycle(GameObject o);
        public abstract void Remove(GameObject o);
    }

    /// <summary>
    /// ScriptableObject working as pooling system for GameObjects. Useful for sharing the same pool over different behaviours.
    /// </summary>
    [CreateAssetMenu(menuName = "Prsd/Utility/Pool Object", fileName = "NewPoolObject")]
    public class Prsd_PoolObject : Prsd_Pooler
    {
        [Tooltip("GameObject to be instantiated by the pool.")]
        [SerializeField] internal GameObject prefab;
        [Tooltip("Max number of objects the pool can contain.")]
        [SerializeField] internal int capacity = 100;
        [Tooltip("Number of objects that get preventively instantiated when the pool is initialized.")]
        [SerializeField] internal int backlog = 0;
        [Tooltip("If pool objects have to be placed in a manually defined scene.")]
        [SerializeField, HorizontalGroup] internal bool useCustomScene;
        [Tooltip("Name of the scene containing pool objects.")]
        [SerializeField, HorizontalGroup, ShowIf(nameof(useCustomScene)), LabelWidth(80f)] internal string sceneName;

        Prsd_DynamicPool<Prsd_Pooled> pool;
        Scene scene;

        [PropertySpace, ShowInInspector, ReadOnly]
        int debugCount => pool != null ? pool.ActivesCount : 0;
        [ShowInInspector, ReadOnly]
        int debugInactivesCount => pool != null ? pool.InactivesCount : 0;

        /// <summary>
        /// Prefab used by the pool to generate instances.
        /// </summary>
        public GameObject Prefab
        {
            get => prefab;
            internal set { if (pool == null) prefab = value; }
        }

        /// <summary>
        /// Returns an array copy of all active GameObjects in the pool.
        /// </summary>
        public GameObject[] ActiveObjects
        {
            get
            {
                var pooled = pool.Actives;
                GameObject[] actives = new GameObject[pooled.Length];
                for (int i = 0; i < actives.Length; i++) actives[i] = pooled[i].gameObject;
                return actives;
            }
        }

        /// <summary>
        /// Can be assigned to perform an action after a new object is created by the pool. Passes the created object as argument.
        /// </summary>
        public Action<GameObject> OnCreate { get; set; }
        /// <summary>
        /// Can be assigned to perform an action before an object is destroyed by the pool. Passes the object as argument.
        /// </summary>
        public Action<GameObject> BeforeDestroy { get; set; }

        /// <summary>
        /// Number of pool objects currently in use.
        /// </summary>
        public int Count => pool.ActivesCount;

        /// <summary>
        /// Max objects the pool can handle.
        /// </summary>
        public int Capacity => capacity;

        /// <summary>
        /// Initializes the pool, finding or creating the scene and instantiating backlog objects.
        /// </summary>
        public void Init()
        {
            if (Application.isPlaying && pool == null && prefab)
            {
                if (useCustomScene)
                {
                    InitScene();
                }
                pool = new Prsd_DynamicPool<Prsd_Pooled>(capacity, backlog, CreateNewElement, DestroyElement, ActivateElement, DeactivateElement);
            }
        }

        void InitScene()
        {
            string sceneName = "POOL: " + (this.sceneName == "" ? "General" : this.sceneName);

            scene = SceneManager.GetSceneByName(sceneName);
            if (!scene.IsValid())
            {
                scene = SceneManager.CreateScene(sceneName);
            }
        }

        void OnDisable()
        {
            if (pool != null)
            {
                pool.Clear();
                pool = null;
            }
        }

        Prsd_Pooled CreateNewElement()
        {
            var o = Instantiate(prefab).AddComponent<Prsd_Pooled>();
            if (useCustomScene)
            {
                if (!scene.IsValid()) InitScene();
                SceneManager.MoveGameObjectToScene(o.gameObject, scene);
            }
            o.Pooler = this;
            o.Prefab = prefab;
            OnCreate?.Invoke(o.gameObject);
            return o;
        }

        void DestroyElement(Prsd_Pooled o)
        {
            if (!o.IsBeeingDestroyedByPool)
            {
                o.IsBeeingDestroyedByPool = true;
                BeforeDestroy?.Invoke(o.gameObject);
                Destroy(o.gameObject);
            }
        }

        void ActivateElement(Prsd_Pooled o)
        {
            o.gameObject.SetActive(true);
        }

        void DeactivateElement(Prsd_Pooled o)
        {
            if (useCustomScene && !o.transform.parent)
            {
                if (!scene.IsValid()) InitScene();
                if (scene.isLoaded)
                    SceneManager.MoveGameObjectToScene(o.gameObject, scene);
            }
            o.gameObject.SetActive(false);
        }

        /// <summary>
        /// Returns an object from the pool.
        /// </summary>
        public override GameObject Get()
        {
            if (pool == null)
            {
                Init();
            }
            if (pool != null)
            {
                return pool.Get().gameObject;
            }
            return null;
        }

        /// <summary>
        /// Returns an object from the pool, setting its position and rotation.
        /// </summary>
        public GameObject Get(Vector3 position, Quaternion rotation)
        {
            var o = Get();
            if (o)
            {
                o.transform.SetPositionAndRotation(position, rotation);
            }
            return o;
        }

        /// <summary>
        /// Recycles an object into the pool.
        /// </summary>
        public override void Recycle(GameObject o)
        {
            if (o && o.TryGetComponent(out Prsd_Pooled p) && p.Pooler == this && pool != null)
            {
                pool.Recycle(p);
            }
        }

        /// <summary>
        /// Recycles all objects in the pool.
        /// </summary>
        public void RecycleAll()
        {
            pool?.RecycleAll();
        }

        /// <summary>
        /// Recycles all objects in the pool except for the ones included as parameters.
        /// </summary>
        public void RecycleAllBut(params GameObject[] toSpare)
        {
            if (pool != null)
            {
                var actives = pool.Actives;
                var toSpareList = new List<GameObject>(toSpare);
                for (int i = 0; i < actives.Length; i++)
                {
                    if (!toSpareList.Contains(actives[i].gameObject)) pool.Recycle(actives[i]);
                }
            }
        }

        /// <summary>
        /// Removes an object from the pool, destroying  it.
        /// </summary>
        public override void Remove(GameObject o)
        {
            if (o && o.TryGetComponent(out Prsd_Pooled p) && p.Pooler == this && pool != null)
            {
                pool.Remove(p);
            }
        }

        /// <summary>
        /// Removes all objects from the pool.
        /// </summary>
        public void Clear()
        {
            pool?.Clear();
        }
    }
