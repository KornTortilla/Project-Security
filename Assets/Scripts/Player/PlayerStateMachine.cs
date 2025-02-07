using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class PlayerStateMachine : MonoBehaviour
    {
        [HideInInspector] public PlayerCharacterController characterController;
        [HideInInspector] public InputBank inputBank;
        [HideInInspector] public Animator animator;
        [HideInInspector] public HitboxController hitboxController;

        [SerializeField] private BasePlayerAttackData[] attackDatas;

        private BaseState currentState;

        private bool canCancel;
        private int attackIndex;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            characterController = GetComponent<PlayerCharacterController>();
            inputBank = GetComponent<InputBank>();
            hitboxController = GetComponent<HitboxController>();

            SetStateToDefault();
        }

        public void SetStateToDefault()
        {
            SetState(new IdleState());
        }

        public bool TryNewState(BaseState newState)
        {
            if (!canCancel) return false;

            SetState(newState);
            return true;
        }

        public void SetState(BaseState newState)
        {
            // if (currentState != null) currentState.Exit();

            if(newState.GetType() ==  typeof(IdleState))
                canCancel = true;
            else
                canCancel = false;

            currentState = newState;
            currentState.stateMachine = this;
            currentState.Enter();
        }

        private void Update()
        {
            currentState.Update();

            CheckInputs();
        }

        private void CheckInputs()
        {
            if (inputBank.LastButtonInput != ButtonInput.Attack) return;

            BasePlayerAttackData attackData = attackDatas[attackIndex];

            if (attackIndex + 1 != attackDatas.Length)
                attackIndex++;
            else
                attackIndex = 0;

            bool setState = TryNewState(new AttackState(attackData));

            if(setState)
                inputBank.ConsumeLastButtonInput();
        }

        public void CanCancel()
        {
            canCancel = true;
        }

        public void ExitState()
        {
            currentState.Exit();
        }
    }
}