using System;

namespace ProjectSecurity.Gameplay
{
    public class SpecialAction
    {
        public ActionData data;
        private float timer;

        public bool onCooldown;

        public static Action<float> OnTimeUpdate;
        public static Action<SpecialAction> OnCooldownEnd;

        public SpecialAction(ActionData data)
        {
            this.data = data;
        }

        public void StartCooldown()
        {
            timer = data.rechargeDuration;

            onCooldown = true;
        }

        public float Tick(float deltaTime)
        {
            timer -= deltaTime;

            if (timer <= 0)
            {
                onCooldown = false;
                return 1f;
            }

            return 1f - (timer / data.rechargeDuration);
        }
    }
}
