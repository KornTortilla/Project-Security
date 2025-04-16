using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class PlayerBody : MonoBehaviour
    {
        private int airRollMax = 1;
        public int airRollCount;

        public bool hasWallJumped;

        public void OnEnable()
        {
            PlayerCharacterController.OnLand += RefreshAirRolls;
        }

        public void OnDisable()
        {
            PlayerCharacterController.OnLand += RefreshAirRolls;
        }

        private void Awake()
        {
            RefreshAirRolls();
        }

        public void UseAirRoll()
        {
            airRollCount--;
        }

        public void RefreshAirRolls()
        {
            airRollCount = airRollMax;
        }
    }
}
