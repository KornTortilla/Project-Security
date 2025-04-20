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
            PlayerCharacterController.OnLand += LandRefresh;
        }

        public void OnDisable()
        {
            PlayerCharacterController.OnLand += LandRefresh;
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

        public void UseWallJump()
        {
            hasWallJumped = true;
        }

        public void RefreshWallJump()
        {
            hasWallJumped = false;
        }

        private void LandRefresh()
        {
            RefreshAirRolls();
            RefreshWallJump();
        }
    }
}
