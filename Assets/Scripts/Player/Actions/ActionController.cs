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

        private List<SpecialAction> specialActions = new List<SpecialAction>();

        private int index;

        private void Start()
        {
            hitboxController = GetComponent<HitboxController>();
            hitboxController.InitializeActionHitboxList(actionDatas);

            view.Initialize(actionDatas);

            for (int i = 0; i < actionDatas.Length; i++)
            {
                specialActions.Add(new SpecialAction(actionDatas[i]));
            }
        }

        public ActionData ActivateAction()
        {
            SpecialAction specialAction = specialActions[index];

            if (specialAction.onCooldown) return null;

            specialAction.StartCooldown();

            hitboxController.SetCurrentAction(specialAction.data);

            index = IntUtility.Wrap(index + 1, actionDatas.Length - 1);

            view.Scroll(index, -1);

            return specialAction.data;
        }

        public void Scroll(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                int direction = Mathf.RoundToInt(context.ReadValue<float>());

                index = IntUtility.Wrap(index + direction, actionDatas.Length - 1);

                // Debug.Log("Direction: " + direction);

                view.Scroll(index, -direction);
            }
        }

        private void Update()
        {
            for (int i = 0; i < specialActions.Count; i++)
            {
                if (!specialActions[i].onCooldown) continue;

                float progress = specialActions[i].Tick(Time.deltaTime);

                view.UpdateImageProgress(i, progress);
            }
        }
    }
}
