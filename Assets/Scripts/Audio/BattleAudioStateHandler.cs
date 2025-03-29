using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class BattleAudioStateHandler : MonoBehaviour
    {
        [SerializeField] private AK.Wwise.State outofCombatState;
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

        private void Start()
        {
            outofCombatState.SetValue();
        }

        public void SetInCombatState()
        {
            inCombatState.SetValue();
        }

        [ContextMenu("Set Lead Up")]
        public void SetLeadUpState()
        {
            leadUpState.SetValue();
        }

        [ContextMenu("Set Perfect")]
        public void SetPerfectState()
        {
            perfectState.SetValue();
        }
    }
}
