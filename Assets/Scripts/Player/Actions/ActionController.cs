using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectSecurity.Gameplay
{
    public class ActionController : MonoBehaviour
    {
        [SerializeField] private ActionData[] actionDatas;
        [SerializeField] private ActionView view;

        private HitboxController hitboxController;
        private ProjectileSpawner projectileSpawner;
        private PlayerHealth playerHealth;

        private List<SpecialAction> specialActions = new List<SpecialAction>();

        private int index;

        private void Start()
        {
            hitboxController = GetComponent<HitboxController>();
            projectileSpawner = GetComponent<ProjectileSpawner>();
            playerHealth = GetComponent<PlayerHealth>();

            for (int i = 0; i < actionDatas.Length; i++)
            {
                ActionData actionData = actionDatas[i];

                specialActions.Add(new SpecialAction(actionData));
                SetupNewAction(actionData);
            }
        }

        private void SetupNewAction(ActionData actionData)
        {
            view.AddAction(actionData.actionName);
            hitboxController.AddHitboxList(actionData);
            projectileSpawner.AddProjectileList(actionData);
        }

        public ActionData ActivateAction()
        {
            SpecialAction specialAction = specialActions[index];

            if (specialAction.charges  < 1) return null;
            if (specialAction.canSelfHarmCancel)
                playerHealth.TakeNonKnockbackDamage(5f);

            specialAction.Use();

            hitboxController.SetCurrentAction(specialAction.data);
            projectileSpawner.SetCurrentAction(specialAction.data);

            index = IntUtility.Wrap(index + 1, specialActions.Count - 1);

            view.Scroll(index, -1);

            return specialAction.data;
        }

        public void Scroll(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                int direction = Mathf.RoundToInt(context.ReadValue<float>());

                index = IntUtility.Wrap(index + direction, specialActions.Count - 1);

                // Debug.Log("Direction: " + direction);

                view.Scroll(index, -direction);
            }
        }

        private void Update()
        {
            for (int i = 0; i < specialActions.Count; i++)
            {
                if (!specialActions[i].recharing) continue;

                float progress = specialActions[i].Tick(Time.deltaTime);

                view.UpdateImageProgress(i, progress);
            }
        }

        public void AddSpecialAction(SpecialAction specialAction)
        {
            Debug.Log("New Special Action: " + specialAction.data.actionName);
            specialActions.Add(specialAction);

            SetupNewAction(specialAction.data);
        }
    }
}
