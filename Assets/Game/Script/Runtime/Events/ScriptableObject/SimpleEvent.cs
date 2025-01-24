using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Events/Simple Event", fileName = "Simple Event", order = 0)]
    public class SimpleEvent : ScriptableObject
    {
        public UnityEvent OnEventRaised;

        public void RaiseEvent()
        {
            OnEventRaised?.Invoke();
        }
    }
}
