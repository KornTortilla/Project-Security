using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectSecurity.Gameplay
{
    public class MeterManager : MonoBehaviour
    {
        [Header("Required Components")]
        [SerializeField] private SliderUI meterUI;

        [Header("Settings")]
        [SerializeField] private int startingMeter = 100;
        [SerializeField] private float tickTime = 0.2f;
        [SerializeField] private int meterLossPerTick = 1;

        private int meter = 0;
        public int Meter 
        { 
            get { return meter; }
        }

        private float timer = 0f;

        private PlayerStateMachine playerStateMachine;
        private PlayerCharacterController playerCharacterController;
        private Animator animator;

        private void Start()
        {
            playerStateMachine = GetComponent<PlayerStateMachine>();
            playerCharacterController = GetComponent<PlayerCharacterController>();
            animator = GetComponent<Animator>();

            ChangeMeter(startingMeter);
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if(timer > tickTime)
            {
                timer = 0f;

                ChangeMeter(-meterLossPerTick);
            }
        }

        public void ChangeMeter(int difference)
        {
            meter += difference;
            if (meter > 100)
                meter = 100;
            else if (meter < 0)
                meter = 0;

            meterUI.Update(meter);
        }

        public bool TryCancel()
        {
            if (meter >= 30 && !TimeManager.main.Frozen)
            {
                ChangeMeter(-30);

                TimeManager.main.Freeze(0.2f);

                return true;
            }

            return false;
        }
    }
}
