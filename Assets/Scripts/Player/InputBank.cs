using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectSecurity.Gameplay
{
    public enum ButtonInput
    {
        None,
        Attack,
        Dash,
        Activate
    }

    public class InputBank : MonoBehaviour
    {
        [SerializeField]
        private float bufferTime = 0.2f;

        public Vector2 RawMoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool JumpTriggered { get; private set; }
        public bool AttackTriggered { get; private set; }
        public bool DashTriggered { get; private set; }
        public bool ActivationTriggered { get; private set; }

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

        public static Action PauseTriggered;

        public ButtonInput LastButtonInput { get; private set; }

        private Coroutine bufferLastInputCoroutine;

        public Vector3 CameraMoveInput
        {
            get
            {
                Vector3 cameraForward = Camera.main.transform.forward;
                cameraForward.y = 0f;
                cameraForward.Normalize();

                Vector3 cameraRight = Camera.main.transform.right;
                cameraRight.y = 0f;
                cameraRight.Normalize();

                Vector3 forwardMovementInput = cameraForward * RawMoveInput.y;
                Vector3 rightMovementInput = cameraRight * RawMoveInput.x;

                return (forwardMovementInput + rightMovementInput).normalized;
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
                RawMoveInput = context.ReadValue<Vector2>();
            else
                RawMoveInput = Vector2.zero;
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (context.performed)
                LookInput = context.ReadValue<Vector2>();
            else
                LookInput = Vector2.zero;
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
                JumpTriggered = true;
            }
            else if (context.canceled)
            {
                JumpTriggered = false;
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                AttackTriggered = true;
                LastButtonInput = ButtonInput.Attack;
                StartBufferLastInput();
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                DashTriggered = true;
                LastButtonInput = ButtonInput.Dash;
                StartBufferLastInput();
            }
        }

        public void OnActivate(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                DashTriggered = true;
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
    }
}
