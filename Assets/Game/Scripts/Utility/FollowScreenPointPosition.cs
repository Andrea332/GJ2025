using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class FollowScreenPointPosition : MonoBehaviour
    {
   
        [SerializeField] private Transform objectToMove;
        private Camera _mainCamera;
        void Awake()
        {
            _mainCamera = Camera.main;
        }

        public void OnPointerMoved(InputAction.CallbackContext context)
        {
            if(_mainCamera == null) return;
            var screenToWorldPoint = _mainCamera.ScreenToWorldPoint(context.action.ReadValue<Vector2>());
            objectToMove.position = new Vector3(screenToWorldPoint.x, screenToWorldPoint.y, 0);
        }
    }
}
