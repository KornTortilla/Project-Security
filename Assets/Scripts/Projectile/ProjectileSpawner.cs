using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class ProjectileSpawner : MonoBehaviour
    {
        private PlayerCharacterController playerCharacterController;

        private Dictionary<string, List<GameObject>> projectileDict;

        private string currentActionName;

        private void Start()
        {
            playerCharacterController = GetComponent<PlayerCharacterController>();
        }

        public void InitializeProjectiles(ActionData[] actionDatas)
        {
            projectileDict = new Dictionary<string, List<GameObject>>();

            foreach (ActionData actionData in actionDatas)
            {
                if (projectileDict.ContainsKey(actionData.actionName)) continue;
                if (actionData.projectileObjects.Length == 0) continue;

                List<GameObject> projectileList = new List<GameObject>();
                foreach (GameObject projectilePrefab in actionData.projectileObjects)
                {
                    projectileList.Add(projectilePrefab);

                    Debug.Log(actionData.actionName);
                }

                projectileDict.Add(actionData.actionName, projectileList);
            }
        }

        public void SetCurrentAction(ActionData actionData)
        {
            currentActionName = actionData.actionName;
        }

        public void Spawn()
        {
            Vector3 position = transform.position + playerCharacterController.CharacterForward;
            position.y = 1f;

            GameObject projectile = Instantiate(projectileDict[currentActionName][0], position, Quaternion.identity);
            projectile.GetComponent<ProjectileSimpleMove>().SetDirection(playerCharacterController.CharacterForward);
            projectile.GetComponent<ProjectileRepeatHitbox>().SetDirection(playerCharacterController.CharacterForward);
        }
    }
}
