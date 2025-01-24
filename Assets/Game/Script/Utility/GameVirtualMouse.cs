using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game
{
    public class GameVirtualMouse : MonoBehaviour
    {
    
        [Header("Cursor")]
        [Tooltip("Whether the component should set the cursor position of the hardware mouse cursor, if one is available. If so, "
                 + "the software cursor pointed (to by 'Cursor Graphic') will be hidden.")]
        [SerializeField] private CursorMode cursorMode;
    
        [Tooltip("The graphic that represents the software cursor. This is hidden if a hardware cursor (see 'Cursor Mode') is used.")]
        [SerializeField] private Graphic cursorGraphic;
    
        [SerializeField] private Canvas canvas;
    
        [Tooltip("The transform for the software cursor. Will only be set if a software cursor is used (see 'Cursor Mode'). Moving the cursor "
                 + "updates the anchored position of the transform.")]
        [SerializeField] private RectTransform cursorTransform;

   
        [Header("Motion")]
        [Tooltip("Speed in pixels per second with which to move the cursor. Scaled by the input from 'Stick Action'.")]
        [SerializeField] private float cursorSpeed = 400;
   
        [Tooltip("Scale factor to apply to 'Scroll Wheel Action' when setting the mouse 'scrollWheel' control.")]
        [SerializeField] private float scrollSpeed = 45;

        [FormerlySerializedAs("m_StickAction")]
        [Space(10)]
        [Tooltip("Vector2 action that moves the cursor left/right (X) and up/down (Y) on screen.")]
        [SerializeField] private InputActionProperty stickAction;
  
        [Tooltip("Button action that triggers a left-click on the mouse.")]
        [SerializeField] private InputActionProperty leftButtonAction;
  
        [Tooltip("Button action that triggers a middle-click on the mouse.")]
        [SerializeField] private InputActionProperty middleButtonAction;

        [Tooltip("Button action that triggers a right-click on the mouse.")]
        [SerializeField] private InputActionProperty rightButtonAction;

        [Tooltip("Button action that triggers a forward button (button #4) click on the mouse.")]
        [SerializeField] private InputActionProperty forwardButtonAction;
   
        [Tooltip("Button action that triggers a back button (button #5) click on the mouse.")]
        [SerializeField] private InputActionProperty mackButtonAction;
   
        [Tooltip("Vector2 action that feeds into the mouse 'scrollWheel' action (scaled by 'Scroll Speed').")]
        [SerializeField] private InputActionProperty scrollWheelAction;
    
 

        private CanvasScaler m_CanvasScaler;
        private Mouse m_VirtualMouse;
        private Mouse m_SystemMouse;
        private Action m_AfterInputUpdateDelegate;
        private Action<InputAction.CallbackContext> m_ButtonActionTriggeredDelegate;
        private double m_LastTime;
        private Vector2 m_LastStickValue;
        private Camera m_Camera;
        
        protected void OnEnable()
        {
            m_Camera = Camera.main;
        
            m_CanvasScaler = canvas.GetComponent<CanvasScaler>();
       
            // Hijack system mouse, if enabled.
            if (cursorMode == CursorMode.HardwareCursorIfAvailable)
                TryEnableHardwareCursor();

            // Add mouse device.
            if (m_VirtualMouse == null)
                m_VirtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
            else if (!m_VirtualMouse.added)
                InputSystem.AddDevice(m_VirtualMouse);

            // Set initial cursor position.
            if (cursorTransform != null)
            {
                var position = cursorTransform.anchoredPosition;
                InputState.Change(m_VirtualMouse.position, position);
                m_SystemMouse?.WarpCursorPosition(position);
            }

            // Hook into input update.
            if (m_AfterInputUpdateDelegate == null)
                m_AfterInputUpdateDelegate = OnAfterInputUpdate;
            InputSystem.onAfterUpdate += m_AfterInputUpdateDelegate;

            // Hook into actions.
            if (m_ButtonActionTriggeredDelegate == null)
                m_ButtonActionTriggeredDelegate = OnButtonActionTriggered;
            SetActionCallback(leftButtonAction, m_ButtonActionTriggeredDelegate, true);
            SetActionCallback(rightButtonAction, m_ButtonActionTriggeredDelegate, true);
            SetActionCallback(middleButtonAction, m_ButtonActionTriggeredDelegate, true);
            SetActionCallback(forwardButtonAction, m_ButtonActionTriggeredDelegate, true);
            SetActionCallback(mackButtonAction, m_ButtonActionTriggeredDelegate, true);

            // Enable actions.
            stickAction.action?.Enable();
            leftButtonAction.action?.Enable();
            rightButtonAction.action?.Enable();
            middleButtonAction.action?.Enable();
            forwardButtonAction.action?.Enable();
            mackButtonAction.action?.Enable();
            scrollWheelAction.action?.Enable();
        }
    
        protected void OnDisable()
        {
            // Remove mouse device.
            if (m_VirtualMouse != null && m_VirtualMouse.added)
            {
                InputSystem.RemoveDevice(m_VirtualMouse);
                m_VirtualMouse = null;
            }
            

            // Let go of system mouse.
            if (m_SystemMouse != null)
            {
                InputSystem.EnableDevice(m_SystemMouse);
                m_SystemMouse = null;
            }

            // Remove ourselves from input update.
            if (m_AfterInputUpdateDelegate != null)
                InputSystem.onAfterUpdate -= m_AfterInputUpdateDelegate;

            // Disable actions.
            stickAction.action?.Disable();
            leftButtonAction.action?.Disable();
            rightButtonAction.action?.Disable();
            middleButtonAction.action?.Disable();
            forwardButtonAction.action?.Disable();
            mackButtonAction.action?.Disable();
            scrollWheelAction.action?.Disable();

            // Unhock from actions.
            if (m_ButtonActionTriggeredDelegate != null)
            {
                SetActionCallback(leftButtonAction, m_ButtonActionTriggeredDelegate, false);
                SetActionCallback(rightButtonAction, m_ButtonActionTriggeredDelegate, false);
                SetActionCallback(middleButtonAction, m_ButtonActionTriggeredDelegate, false);
                SetActionCallback(forwardButtonAction, m_ButtonActionTriggeredDelegate, false);
                SetActionCallback(mackButtonAction, m_ButtonActionTriggeredDelegate, false);
            }

            m_LastTime = default;
            m_LastStickValue = default;
        }
    
        private void TryEnableHardwareCursor()
        {
            var devices = InputSystem.devices;
            for (var i = 0; i < devices.Count; ++i)
            {
                var device = devices[i];
                if (device.native && device is Mouse mouse)
                {
                    m_SystemMouse = mouse;
                    break;
                }
            }

            if (m_SystemMouse == null)
            {
                if (cursorGraphic != null)
                    cursorGraphic.enabled = true;
                return;
            }

            InputSystem.DisableDevice(m_SystemMouse);

            // Sync position.
            if (m_VirtualMouse != null)
                m_SystemMouse.WarpCursorPosition(m_VirtualMouse.position.value);

            // Turn off mouse cursor image.
            if (cursorGraphic != null)
                cursorGraphic.enabled = false;
        }
    
        private void UpdateMotion()
        {
            if (m_VirtualMouse == null)
                return;

            // Read current stick value.
            var stickAction = this.stickAction.action;
            if (stickAction == null)
                return;
            var stickValue = stickAction.ReadValue<Vector2>();
            if (Mathf.Approximately(0, stickValue.x) && Mathf.Approximately(0, stickValue.y))
            {
                // Motion has stopped.
                m_LastTime = default;
                m_LastStickValue = default;
            }
            else
            {
                var resolutionXScale = canvas.pixelRect.xMax / m_CanvasScaler.referenceResolution.x ;
                var resolutionYScale = canvas.pixelRect.yMax / m_CanvasScaler.referenceResolution.y ;

                var currentTime = InputState.currentTime;
                if (Mathf.Approximately(0, m_LastStickValue.x) && Mathf.Approximately(0, m_LastStickValue.y))
                {
                    // Motion has started.
                    m_LastTime = currentTime;
                }

                // Compute delta.
                var deltaTime = (float)(currentTime - m_LastTime);
                var delta = new Vector2(cursorSpeed * resolutionXScale * stickValue.x * deltaTime, cursorSpeed * resolutionYScale * stickValue.y * deltaTime);

                // Update position.
                var currentPosition = m_VirtualMouse.position.value;
                var newPosition = currentPosition + delta;

                ////REVIEW: for the hardware cursor, clamp to something else?
                // Clamp to canvas.
                if (canvas != null)
                {
                    // Clamp to canvas.
                    var pixelRect = canvas.pixelRect;
                    newPosition.x = Mathf.Clamp(newPosition.x, pixelRect.xMin, pixelRect.xMax);
                    newPosition.y = Mathf.Clamp(newPosition.y, pixelRect.yMin, pixelRect.yMax);
                }

                ////REVIEW: the fact we have no events on these means that actions won't have an event ID to go by; problem?
                InputState.Change(m_VirtualMouse.position, newPosition);
                InputState.Change(m_VirtualMouse.delta, delta);

                // Update software cursor transform, if any.
                if (cursorTransform != null &&
                    (cursorMode == CursorMode.SoftwareCursor ||
                     (cursorMode == CursorMode.HardwareCursorIfAvailable && m_SystemMouse == null)))
                {
                    //Vector2 newPointerPosition = new(newPosition.x / resolutionXScale, newPosition.y / resolutionYScale);
                    if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                    {
                        cursorTransform.position = newPosition;
                    }
                    else if (canvas.renderMode == RenderMode.WorldSpace)
                    {
                        var screenToWorldPoint = m_Camera.ScreenToWorldPoint(newPosition);
                        cursorTransform.position = new Vector2(screenToWorldPoint.x, screenToWorldPoint.y);
                    }
                }
                
                m_LastStickValue = stickValue;
                m_LastTime = currentTime;

                // Update hardware cursor.
                m_SystemMouse?.WarpCursorPosition(newPosition);
            }

            // Update scroll wheel.
            var scrollAction = scrollWheelAction.action;
            if (scrollAction != null)
            {
                var scrollValue = scrollAction.ReadValue<Vector2>();
                scrollValue.x *= scrollSpeed;
                scrollValue.y *= scrollSpeed;

                InputState.Change(m_VirtualMouse.scroll, scrollValue);
            }
        }
    
        private void OnButtonActionTriggered(InputAction.CallbackContext context)
        {
            if (m_VirtualMouse == null)
                return;

            // The button controls are bit controls. We can't (yet?) use InputState.Change to state
            // the change of those controls as the state update machinery of InputManager only supports
            // byte region updates. So we just grab the full state of our virtual mouse, then update
            // the button in there and then simply overwrite the entire state.

            var action = context.action;
            MouseButton? button = null;
            if (action == leftButtonAction.action)
                button = MouseButton.Left;
            else if (action == rightButtonAction.action)
                button = MouseButton.Right;
            else if (action == middleButtonAction.action)
                button = MouseButton.Middle;
            else if (action == forwardButtonAction.action)
                button = MouseButton.Forward;
            else if (action == mackButtonAction.action)
                button = MouseButton.Back;

            if (button != null)
            {
                var isPressed = context.control.IsPressed();
                m_VirtualMouse.CopyState<MouseState>(out var mouseState);
                mouseState.WithButton(button.Value, isPressed);

                InputState.Change(m_VirtualMouse, mouseState);
            }
        }
    
        private static void SetActionCallback(InputActionProperty field, Action<InputAction.CallbackContext> callback, bool install = true)
        {
            var action = field.action;
            if (action == null)
                return;

            // We don't need the performed callback as our mouse buttons are binary and thus
            // we only care about started (1) and canceled (0).

            if (install)
            {
                action.started += callback;
                action.canceled += callback;
            }
            else
            {
                action.started -= callback;
                action.canceled -= callback;
            }
        }
        private static void SetAction(ref InputActionProperty field, InputActionProperty value)
        {
            var oldValue = field;
            field = value;

            if (oldValue.reference == null)
            {
                var oldAction = oldValue.action;
                if (oldAction != null && oldAction.enabled)
                {
                    oldAction.Disable();
                    if (value.reference == null)
                        value.action?.Enable();
                }
            }
        }
    
        private void OnAfterInputUpdate()
    
        {
            UpdateMotion();
        }
        public void OnControlSchemeChanged(PlayerInput playerInput)
        {
            if(playerInput.currentControlScheme == "GamePad")
            {
                cursorGraphic.enabled = true;
                Cursor.visible = false;
                return;
            }
            cursorGraphic.enabled = false;
            Cursor.visible = true;
        }
        
        public enum CursorMode
        {
            SoftwareCursor,
            HardwareCursorIfAvailable,
        }
    }
}

