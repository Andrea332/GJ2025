using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game.DataEvent.Component
{
    public class DataEventObjectToTransformExposer : DataEventObjectToClassExposer<Transform>
    {
        private protected override void OnDataEventTriggered(Object arg0)
        {
            onDataEvent?.Invoke(((GameObject)arg0).transform);
        }
    }
}
