using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Game
{
    public class PointerToGameRaycast : MonoBehaviour
    {
        private Camera _mainCamera;
        private Ray _ray;
        private Vector2 _screenPoint;
        private Collider2D _colliderHitted;

        void Awake()
        {
            _mainCamera = Camera.main;
        }
        public void OnPointerMoved(InputAction.CallbackContext context)
        {
            _screenPoint = context.ReadValue<Vector2>();
        }
        public void OnInteractWithObject(InputAction.CallbackContext context)
        {
            if(!context.canceled) return;
          
            if(_mainCamera == null) return;
            
            var worldPoint = _mainCamera.ScreenToWorldPoint(_screenPoint);
            
            _colliderHitted = Physics2D.OverlapPoint(worldPoint);
            
            if (_colliderHitted != null)
            {
                var interactableComponent = _colliderHitted.GetComponentInParent<IInteractAble>();
                if(interactableComponent != null)
                    interactableComponent.Interact(worldPoint);
                //Debug.Log("Pointer Collided with: " + _raycastHit2D.collider.name);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_ray.origin, _ray.direction * 100);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_screenPoint, 0.3f);
        }
        
    }
}
