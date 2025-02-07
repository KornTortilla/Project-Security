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
        Activate
    }

    public class InputBank : MonoBehaviour
    {
        [SerializeField]
        private float bufferTime = 0.2f;

        public static Action PauseTriggered;

        public Vector2 RawMoveInput { get; private set; }

        public Vector3 CameraMoveInput { get; private set; }

        public ButtonInput LastButtonInput { get; private set; }

        public bool JumpHeld { get; private set; }

        private Coroutine bufferLastInputCoroutine;

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

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                RawMoveInput = context.ReadValue<Vector2>();
                SetMovementToCamera();
            }
            else
            {
                RawMoveInput = Vector2.zero;
                CameraMoveInput = Vector3.zero;
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

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                PauseTriggered?.Invoke();
            }
        }

        private void SetMovementToCamera()
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            Vector3 cameraRight = Camera.main.transform.right;
            cameraRight.y = 0f;
            cameraRight.Normalize();

            Vector3 forwardMovementInput = cameraForward * RawMoveInput.y;
            Vector3 rightMovementInput = cameraRight * RawMoveInput.x;

            CameraMoveInput = (forwardMovementInput + rightMovementInput).normalized;
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
    }
}
