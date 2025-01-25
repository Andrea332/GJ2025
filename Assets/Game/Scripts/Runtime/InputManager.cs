using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] InputActionReference interactAction;
    [SerializeField] InputActionReference pointAction;

    Camera _mainCamera;
    Ray _ray;
    Vector2 _screenPoint;
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
        _screenPoint = context.ReadValue<Vector2>();
    }

    void OnActionperformed(InputAction.CallbackContext context)
    {
        if (!_mainCamera) return;
        if (IsOverUIObject(_screenPoint)) return;

        var worldPoint = _mainCamera.ScreenToWorldPoint(_screenPoint);

        _colliderHitted = Physics2D.OverlapPoint(worldPoint);

        if (_colliderHitted != null)
        {
            if (_colliderHitted.TryGetComponent<Interactable>(out var interactableComponent))
            {
                interactableComponent.Interact(worldPoint);
            }
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
