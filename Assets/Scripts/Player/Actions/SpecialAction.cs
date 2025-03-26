using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public enum SpecialActionStat
    {
        FlatDamange,
        PercentDamage,
        AnimationSpeed,
        RechargeSpeed,
        MaxCharges
    }

    public class SpecialAction
    {
        public static Action<float> OnTimeUpdate;
        public static Action<SpecialAction> OnCooldownEnd;

        public ActionData data;

        private float timer;
        private float rechargeTime;

        public bool recharing;
        public bool canSelfHarmCancel;

        public int charges;
        private int maxCharges = 1;

        public float speed = 1;

        private List<ActionAttributeData> actionAttributes;

        public SpecialAction(ActionData data)
        {
            this.data = data;

            charges = maxCharges;
            rechargeTime = data.rechargeDuration;
        }

        public string AddRandomAttribute()
        {
            actionAttributes = new List<ActionAttributeData>();
            ActionAttributeData[] actionAttirubteDatas = ContentLoader.ActionAttributeList;

            System.Random random = new System.Random();
            ActionAttributeData actionAttirubteData = actionAttirubteDatas[random.Next(0, actionAttirubteDatas.Length)];

            switch (actionAttirubteData.attributeType)
            {
                case (ActionAttributeType.StatChange):
                    StatActionAttributeData statActionAttribute = (StatActionAttributeData)actionAttirubteData;
                    ChangeStat(statActionAttribute.actionStatChanges);
                    break;

                case (ActionAttributeType.CanSelfHarmCancel):
                    canSelfHarmCancel = true;
                    break;
            }

            actionAttributes.Add(actionAttirubteData);

            return actionAttirubteData.description;
        }

        private void ChangeStat(ActionStatChange[] actionStatChanges)
        {
            foreach(ActionStatChange actionStatChange in actionStatChanges)
            {
                switch (actionStatChange.specialActionStat)
                {
                    case (SpecialActionStat.MaxCharges):
                        maxCharges += (int)actionStatChange.arg;
                        Debug.Log("Max Charges: " + maxCharges);
                        break;

                    case (SpecialActionStat.RechargeSpeed):
                        rechargeTime -= rechargeTime * actionStatChange.arg;
                        break;

                    case (SpecialActionStat.AnimationSpeed):
                        speed *= actionStatChange.arg;
                        break;
                }
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
                if (charges < maxCharges)
                    StartRecharging();
                else
                    recharing = false;
                return 1f;
            }

            return 1f - (timer / rechargeTime);
        }
    }
}

