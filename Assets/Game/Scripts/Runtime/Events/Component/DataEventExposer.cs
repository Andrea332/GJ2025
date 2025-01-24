using System;
using Game.DataEvent.ScriptableObject;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class DataEventExposer<T> : MonoBehaviour
    {
        [SerializeField] private DataEvent<T> dataEvent;
    
        public UnityEvent<T> onDataEvent;

        private protected virtual void Awake()
        {
            dataEvent.OnDataChanged.AddListener(OnDataEventTriggered);
        }

        private protected virtual void OnDestroy()
        {
            dataEvent.OnDataChanged.RemoveListener(OnDataEventTriggered);
        }

        private protected virtual void OnDataEventTriggered(T arg0)
        {
            onDataEvent?.Invoke(arg0);
        }
    }
}
