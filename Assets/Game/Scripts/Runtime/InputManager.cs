using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] InputActionReference interactAction;
    [SerializeField] InputActionReference pointAction;
    [SerializeField] InputActionReference quitAction;

    Camera _mainCamera;
    Collider2D _colliderHitted;

    public static Vector2 CursorScreenPosition { get; private set; }
    public static Vector2 CursorWorldPosition { get; private set; }

    void Awake()
    {
        _mainCamera = Camera.main;
    }

    void OnEnable()
    {
        interactAction.action.performed += OnActionperformed;
        pointAction.action.performed += OnPointerMoved;
        quitAction.action.performed += OnQuitPerformed;
    }

    void OnDisable()
    {
        interactAction.action.performed -= OnActionperformed;
        pointAction.action.performed -= OnPointerMoved;
        quitAction.action.performed -= OnQuitPerformed;
    }

    void OnPointerMoved(InputAction.CallbackContext context)
    {
        if (!_mainCamera) return;

        CursorScreenPosition = context.ReadValue<Vector2>();
        CursorWorldPosition = _mainCamera.ScreenToWorldPoint(CursorScreenPosition);
        var newColl = IsOverUIObject(CursorScreenPosition) ? null : Physics2D.OverlapPoint(CursorWorldPosition);

        if (newColl != _colliderHitted)
        {
            if (_colliderHitted && _colliderHitted.TryGetComponent<Interactable>(out var oldInteractable))
            {
                oldInteractable.Hover(false, CursorWorldPosition);
            }

            if (newColl && newColl.TryGetComponent<Interactable>(out var newInteractable))
            {
                newInteractable.Hover(true, CursorWorldPosition);
            }

            _colliderHitted = newColl;
        }
    }

    void OnActionperformed(InputAction.CallbackContext context)
    {
        if (_colliderHitted && _colliderHitted.TryGetComponent<Interactable>(out var interactableComponent))
        {
            interactableComponent.Interact(CursorWorldPosition);
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

    private void OnQuitPerformed(InputAction.CallbackContext obj)
    {
        Application.Quit();
    }
}
