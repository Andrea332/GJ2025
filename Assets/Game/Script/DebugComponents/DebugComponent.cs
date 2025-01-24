using UnityEngine;

namespace Game.Scripts.DebugComponents
{
    public class DebugComponent<T> : MonoBehaviour
    {
        public virtual void ShowDebugValue(T value)
        {
            Debug.Log(value);
        }
    }
}
