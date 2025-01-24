using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionHelper : MonoBehaviour
{
    [SerializeField] private bool enableAtStart;
    [SerializeField] private InputActionReference actionReference;

    public static event Action<InputAction.CallbackContext> OnActionStarted;
    public static event Action<InputAction.CallbackContext> OnActionPerformed;
    public static event Action<InputAction.CallbackContext> OnActionCanceled;

    public void EnableInputAction()
    {
        actionReference.action.Enable();
    }

    public void DisableInputAction()
    {
        actionReference.action.Disable();
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Start()
    {
        if (!enableAtStart) return;
        EnableInputAction();
    }

    public void Subscribe()
    {
        actionReference.action.started += ActionOnStarted;
        actionReference.action.performed += ActionOnPerformed;
        actionReference.action.canceled += ActionOnCanceled;
    }

    public void Unsubscribe()
    {
        actionReference.action.started -= ActionOnStarted;
        actionReference.action.performed -= ActionOnPerformed;
        actionReference.action.canceled -= ActionOnCanceled;
    }

    private void ActionOnStarted(InputAction.CallbackContext obj)
    {
        OnActionStarted?.Invoke(obj);
    }
    private void ActionOnPerformed(InputAction.CallbackContext obj)
    {
        OnActionPerformed?.Invoke(obj);
    }
    private void ActionOnCanceled(InputAction.CallbackContext obj)
    {
        OnActionCanceled?.Invoke(obj);
    }
}
