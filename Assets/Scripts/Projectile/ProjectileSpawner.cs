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

        private void Awake()
        {
            playerCharacterController = GetComponent<PlayerCharacterController>();

            projectileDict = new Dictionary<string, List<GameObject>>();
        }

        public void AddProjectileList(ActionData actionData)
        {
            if (projectileDict.ContainsKey(actionData.actionName)) return;
            if (actionData.projectileObjects == null) return;
            if (actionData.projectileObjects.Length == 0) return;

            List<GameObject> projectileList = new List<GameObject>();
            foreach (GameObject projectilePrefab in actionData.projectileObjects)
            {
                projectileList.Add(projectilePrefab);
            }

            projectileDict.Add(actionData.actionName, projectileList);
        }

        public void SetCurrentAction(ActionData actionData)
        {
            currentActionName = actionData.actionName;
        }

        public void Spawn()
        {
            Vector3 position = transform.position + playerCharacterController.CharacterForward;
            position.y += 1f;

            GameObject projectile = Instantiate(projectileDict[currentActionName][0], position, Quaternion.identity);
            projectile.GetComponent<ProjectileSimpleMove>().SetMoveDirection(playerCharacterController.CharacterForward);
            projectile.GetComponent<ProjectileRepeatHitbox>().SetHitDirection(playerCharacterController.CharacterForward);
        }
    }
}
