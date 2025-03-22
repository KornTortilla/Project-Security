using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public static class ActionDataLoader
    {
        private static ActionData[] actionList;
        public static ActionData[] ActionList
        {
            get
            {
                if (actionList == null)
                    GetCachedActionList();
                return actionList;
            }
        }

        private static void GetCachedActionList()
        {
            string[] guids = AssetDatabase.FindAssets("t:"+typeof(ActionData).Name);

            actionList = new ActionData[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                actionList[i] = (ActionData)AssetDatabase.LoadAssetAtPath(path, typeof(ActionData));
            }
        }
    }
}
