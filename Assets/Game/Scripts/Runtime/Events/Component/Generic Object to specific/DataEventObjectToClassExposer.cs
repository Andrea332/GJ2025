using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game.DataEvent.Component
{
    public abstract class DataEventObjectToClassExposer<T> : MonoBehaviour where T : Object
    {
        [SerializeField] private DataEvent<Object> objectDataEvent;
    
        public UnityEvent<T> onDataEvent;

        private protected virtual void Awake()
        {
            objectDataEvent.OnDataChanged.AddListener(OnDataEventTriggered);
        }

        private protected virtual void OnDestroy()
        {
            objectDataEvent.OnDataChanged.RemoveListener(OnDataEventTriggered);
        }

        private protected virtual void OnDataEventTriggered(Object arg0)
        {
            onDataEvent?.Invoke((T)arg0);
        }
    }
}
