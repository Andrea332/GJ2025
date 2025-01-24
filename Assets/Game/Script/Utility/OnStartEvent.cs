using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class OnStartEvent : MonoBehaviour
    {
        [SerializeField] private UnityEvent onEvent;

        private void Start()
        {
            onEvent?.Invoke();
        }
    }
}
