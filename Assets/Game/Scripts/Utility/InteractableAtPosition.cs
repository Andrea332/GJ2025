using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class InteractableAtPosition : MonoBehaviour, IInteractAble
    { 
        [SerializeField] private SimpleEvent startInteraction;
        [SerializeField] private Transform positionToReach;
        public UnityEvent<Vector3> onInteract;
        public UnityEvent<UnityAction> onInteractEndAction;
        
        public void Interact(Vector3 position)
        {
           if(!enabled) return;
           onInteract.Invoke(positionToReach.position);
           onInteractEndAction?.Invoke(StartInteractionEnd);
        }

        private void StartInteractionEnd()
        {
            startInteraction.RaiseEvent();
        }
    }
}
