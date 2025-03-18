using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class PlayerStateMachine : MonoBehaviour
    {
        [HideInInspector] public PlayerCharacterController characterController;
        [HideInInspector] public InputBank inputBank;
        [HideInInspector] public Animator animator;
        [HideInInspector] public HitboxController hitboxController;
        [HideInInspector] public ActionController actionController;
        [HideInInspector] public LockOnController lockOnController;
        [HideInInspector] public MeterManager meterManager;

        [SerializeField] private BasePlayerAttackData[] groundAttackDatas;
        [SerializeField] private BasePlayerAttackData[] airAttackDatas;

        private BaseState currentState;

        private bool canCancel;
        private int attackIndex;

        private void OnEnable()
        {
            HitboxController.OnHit += NotifyHitboxHit;
        }

        private void OnDisable()
        {
            HitboxController.OnHit -= NotifyHitboxHit;
        }

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            characterController = GetComponent<PlayerCharacterController>();
            inputBank = GetComponent<InputBank>();
            hitboxController = GetComponent<HitboxController>();
            actionController = GetComponent<ActionController>();
            lockOnController = GetComponent<LockOnController>();
            meterManager = GetComponent<MeterManager>();

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

            hitboxController.StopCurrentHitbox();
            characterController.EnableEnemyCollision();

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
            switch(inputBank.LastButtonInput)
            {
                case ButtonInput.Attack:
                    TryAttackState();
                    break;

                case ButtonInput.Dash:
                    TryNewState(new DashState());
                    break;

                case ButtonInput.Activate:
                    TryActionState();
                    break;

                case ButtonInput.Cancel:
                    TryOverrideCancel();
                    break;
            }
        }

        private bool CanCancelCheck()
        {
            return canCancel;
        }

        private void TryAttackState()
        {
            if (!CanCancelCheck()) return;

            inputBank.ConsumeLastButtonInput();

            if (currentState.GetType() == typeof(AttackManagerState))
            {
                AttackManagerState attackManagerState = (AttackManagerState)currentState;
                attackManagerState.Continue();

                canCancel = false;

                return;
            }

            if (characterController.IsGrounded)
            {
                SetState(new AttackManagerState(groundAttackDatas));
            } 
            else
            {
                SetState(new AttackManagerState(airAttackDatas));
            }
        }

        private void TryActionState()
        {
            if (!canCancel) return;

            ActionData actionData = actionController.ActivateAction();

            if (actionData == null) return;

            inputBank.ConsumeLastButtonInput();

            SetState(actionData.InstantiateNewState());
            animator.Play(actionData.animationName, -1, 0f);
        }

        private void TryOverrideCancel()
        {
            if(meterManager.TryCancel())
            {
                SetStateToDefault();
                characterController.OverrideVelocity(0f);
                animator.Play("Idle");

                inputBank.ConsumeLastButtonInput();
            }
        }

        public void Land()
        {
            SetState(new LandState());
        }

        public void Hurt()
        {
            SetState(new HurtState());
        }

        public void NotifyHitboxHit()
        {
            currentState.HandleHitboxHit();
        }

        public void NotifyMove()
        {
            currentState.Move();
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