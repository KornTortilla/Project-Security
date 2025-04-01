using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectSecurity.Gameplay
{
    public enum ButtonInput
    {
        None,
        Jump,
        Attack,
        Dash,
        Activate,
        Cancel
    }

    public class InputBank : MonoBehaviour
    {
        [SerializeField]
        private float bufferTime = 0.2f;

        public static Action PauseTriggered;

        public Vector2 RawMoveInput { get; private set; }

        public Vector2 RawLookInput { get; private set; }

        public Vector3 CameraMoveInput 
        { 
            get
            {
                return GetToCameraVector(RawMoveInput);
            }
        }

        public Vector3 CameraLookInput
        {
            get
            {
                return GetToCameraVector(RawLookInput);
            }
        }

        public ButtonInput LastButtonInput { get; private set; }

        public bool JumpHeld { get; private set; }

        public bool LockOnSwitchHeld { get; private set; }

        private float scrollInput;
        public float ScrollInput
        {
            get
            {
                float scroll = scrollInput;
                scrollInput = 0;
                return scroll;
            }
        }

        private Coroutine bufferLastInputCoroutine;

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                RawMoveInput = context.ReadValue<Vector2>();
            }
            else
            {
                RawMoveInput = Vector2.zero;
            }
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                RawLookInput = context.ReadValue<Vector2>();
            }
            else
            {
                RawLookInput = Vector2.zero;
            }
        }

        public void OnScroll(InputAction.CallbackContext context)
        {
            if (context.performed)
                scrollInput = context.ReadValue<float>();
            else
                scrollInput = 0f;
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                JumpHeld = true;

                LastButtonInput = ButtonInput.Jump;
                StartBufferLastInput();
            }
            else if(context.canceled)
            {
                JumpHeld = false;
            }
        }

        public void OnLockOnSwitch(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                LockOnSwitchHeld = true;
            }
            else if (context.canceled)
            {
                LockOnSwitchHeld = false;
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                LastButtonInput = ButtonInput.Attack;
                StartBufferLastInput();
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                LastButtonInput = ButtonInput.Dash;
                StartBufferLastInput();
            }
        }

        public void OnActivate(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                LastButtonInput = ButtonInput.Activate;
                StartBufferLastInput();
            }
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                LastButtonInput = ButtonInput.Cancel;
                StartBufferLastInput();
            }
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                PauseTriggered?.Invoke();
            }
        }

        private void StartBufferLastInput()
        {
            if (bufferLastInputCoroutine != null) StopCoroutine(bufferLastInputCoroutine);
            bufferLastInputCoroutine = StartCoroutine(BufferInput(() => LastButtonInput = ButtonInput.None));
        }

        private IEnumerator BufferInput(Action action)
        {
            yield return new WaitForSeconds(bufferTime);

            action();
        }

        public void ConsumeLastButtonInput()
        {
            StopCoroutine(bufferLastInputCoroutine);

            LastButtonInput = ButtonInput.None;
        }

        private Vector3 GetToCameraVector(Vector2 input)
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            Vector3 cameraRight = Camera.main.transform.right;
            cameraRight.y = 0f;
            cameraRight.Normalize();

            Vector3 forwardInput = cameraForward * input.y;
            Vector3 rightInput = cameraRight * input.x;

            return (forwardInput + rightInput).normalized;
        }
    }
}
