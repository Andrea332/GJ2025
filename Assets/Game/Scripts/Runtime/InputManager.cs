using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] InputActionReference interactAction;
    [SerializeField] InputActionReference pointAction;

    Camera _mainCamera;
    Vector2 worldPoint;
    Collider2D _colliderHitted;

    void Awake()
    {
        _mainCamera = Camera.main;
    }

    void OnEnable()
    {
        interactAction.action.performed += OnActionperformed;
        pointAction.action.performed += OnPointerMoved;
    }

    void OnDisable()
    {
        interactAction.action.performed -= OnActionperformed;
        pointAction.action.performed -= OnPointerMoved;
    }

    void OnPointerMoved(InputAction.CallbackContext context)
    {
        if (!_mainCamera) return;

        var screenPoint = context.ReadValue<Vector2>();
        worldPoint = _mainCamera.ScreenToWorldPoint(screenPoint);
        var newColl = IsOverUIObject(screenPoint) ? null : Physics2D.OverlapPoint(worldPoint);

        if (newColl != _colliderHitted)
        {
            if (_colliderHitted && _colliderHitted.TryGetComponent<Interactable>(out var oldInteractable))
            {
                oldInteractable.Hover(false, worldPoint);
            }

            if (newColl && newColl.TryGetComponent<Interactable>(out var newInteractable))
            {
                newInteractable.Hover(true, worldPoint);
            }

            _colliderHitted = newColl;
        }
    }

    void OnActionperformed(InputAction.CallbackContext context)
    {
        if (_colliderHitted && _colliderHitted.TryGetComponent<Interactable>(out var interactableComponent))
        {
            interactableComponent.Interact(worldPoint);
        }
    }

    public static bool IsOverUIObject(Vector2 position)
    {
        if (!EventSystem.current) { return false; }
        PointerEventData eventDataCurrentPosition = new(EventSystem.current)
        {
            position = position
        };
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
