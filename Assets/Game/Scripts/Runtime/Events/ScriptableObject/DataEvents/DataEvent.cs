using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class DataEvent<T> : ScriptableObject
    {
        public UnityEvent<T> OnDataChanged;

        public void RaiseOnDataChanged(T data)
        {
            OnDataChanged?.Invoke(data);
        }
    }
}
