using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.DataEvent.ScriptableObject
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Events/Player Input Event", fileName = "Player Input Event", order = 0)]
    public class DataEventPlayerInput : DataEvent<PlayerInput>
    {
    
    }
}
