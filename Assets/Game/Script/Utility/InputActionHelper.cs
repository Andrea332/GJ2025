using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Game
{
    public class InputActionHelper : MonoBehaviour
    {
        [SerializeField] private bool enableAtStart;
        [SerializeField] private InputActionReference actionReference;
        [SerializeField] private UnityEvent<InputAction.CallbackContext> onActionStarted;
        [SerializeField] private UnityEvent<InputAction.CallbackContext> onActionPerformed;
        [SerializeField] private UnityEvent<InputAction.CallbackContext> onActionCanceled;

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
            if(!enableAtStart) return;
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
            onActionStarted?.Invoke(obj);
        }
        private void ActionOnPerformed(InputAction.CallbackContext obj)
        {
            onActionPerformed?.Invoke(obj);
        }
        private void ActionOnCanceled(InputAction.CallbackContext obj)
        {
            onActionCanceled?.Invoke(obj);
        }
    }
}
