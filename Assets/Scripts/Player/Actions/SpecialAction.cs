using System;
using System.Collections.Generic;

namespace ProjectSecurity.Gameplay
{
    public class SpecialAction
    {
        public static Action<float> OnTimeUpdate;
        public static Action<SpecialAction> OnCooldownEnd;

        public ActionData data;
        private float timer;

        public bool recharing;
        public bool canSelfHarmCancel;

        public int charges;
        private int maxCharges = 1;

        private List<ActionAttributeData> actionAttributes;

        public SpecialAction(ActionData data)
        {
            this.data = data;
        }

        public void AddRandomAttribute()
        {
            ActionAttributeData actionAttirubteData = ActionDataLoader.ActionAttributeList[0];

            switch(actionAttirubteData.specialActionStat)
            {

            }
        }

        public void Use()
        {
            charges--;

            StartRecharging();
        }

        private void StartRecharging()
        {
            timer = data.rechargeDuration;

            recharing = true;
        }

        public float Tick(float deltaTime)
        {
            timer -= deltaTime;

            if (timer <= 0)
            {
                charges++;
                if (charges > maxCharges)
                    StartRecharging();
                else
                    recharing = false;
                return 1f;
            }

            return 1f - (timer / data.rechargeDuration);
        }
    }
}
