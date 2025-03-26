using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class BaseState
    {
        public PlayerStateMachine stateMachine;

        protected bool changeStateOnLand = false;
        protected float speed = 1f;

        public PlayerCharacterController characterController
        {
            get { return stateMachine.characterController; }
        }

        public HitboxController hitboxController
        {
            get { return stateMachine.hitboxController; }
        }

        public Animator animator
        {
            get { return stateMachine.animator; }
        }

        public InputBank inputBank
        {
            get { return stateMachine.inputBank; }
        }

        public LockOnController lockOnController
        {
            get { return stateMachine.lockOnController; }
        }

        public virtual void Enter()
        {

        }

        public virtual void Update()
        {
            if (changeStateOnLand && characterController.hasHitGroundThisFrame)
                stateMachine.Land();
        }

        public virtual void Move()
        {

        }

        public virtual void HandleHitboxHit()
        {

        }

        public virtual void Exit()
        {
            stateMachine.SetStateToDefault();
        }

        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }

        public void ReorientToMove()
        {
            if (inputBank.CameraMoveInput.magnitude > 0)
                characterController.OverrideRotation(inputBank.CameraMoveInput);
        }

        public void ReorientToLockOnOrMove()
        {
            if (lockOnController.isLockedOn)
                characterController.OverrideRotation(lockOnController.GetDirectionToTarget());
            else if (inputBank.CameraMoveInput.magnitude > 0)
                characterController.OverrideRotation(inputBank.CameraMoveInput);
        }
    }
}