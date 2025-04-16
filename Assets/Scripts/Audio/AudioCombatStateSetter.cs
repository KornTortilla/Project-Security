using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class AudioCombatStateSetter : MonoBehaviour
    {
        [SerializeField] private AK.Wwise.State outOfCombatState;
        [SerializeField] private AK.Wwise.State inCombatState;
        [SerializeField] private AK.Wwise.State leadUpState;
        [SerializeField] private AK.Wwise.State perfectState;

        private void OnEnable()
        {
            HitboxController.OnHit += SetInCombatState;
        }

        private void OnDisable()
        {
            HitboxController.OnHit -= SetInCombatState;
        }

        private void Awake()
        {
            outOfCombatState.SetValue();
        }

        public void SetInCombatState()
        {
            inCombatState.SetValue();
        }
    }
}
