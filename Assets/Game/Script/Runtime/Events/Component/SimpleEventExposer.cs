using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class SimpleEventExposer : MonoBehaviour
    {
        [SerializeField] private SimpleEvent simpleEvent;
    
        public UnityEvent onEventRaised;

        private void Awake()
        {
            simpleEvent.OnEventRaised.AddListener(OnDataEventTriggered);
        }

        private void OnDestroy()
        {
            simpleEvent.OnEventRaised.RemoveListener(OnDataEventTriggered);
        }

        private void OnDataEventTriggered()
        {
            onEventRaised?.Invoke();
        }
    }
}
