using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class BaseState
    {
        public PlayerStateMachine stateMachine;

        public PlayerCharacterController characterController
        {
            get { return stateMachine.characterController; }
        }

        public Animator animator
        {
            get { return stateMachine.animator; }
        }

        public InputBank inputBank
        {
            get { return stateMachine.inputBank; }
        }

        public virtual void Enter()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void Exit()
        {
            stateMachine.SetStateToDefault();
        }
    }
}