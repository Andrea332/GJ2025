using UnityEngine;

internal class Prsd_Pooled : MonoBehaviour
{
    internal bool IsBeeingDestroyedByPool { get; set; } = false;
    internal Prsd_Pooler Pooler { get; set; }
    internal GameObject Prefab { get; set; }

    void OnDisable()
    {
        if (IsBeeingDestroyedByPool) return;
        Pooler.Recycle(gameObject);
    }

    void OnDestroy()
    {
        if (IsBeeingDestroyedByPool) return;
        Pooler.Remove(gameObject);
    }
}
