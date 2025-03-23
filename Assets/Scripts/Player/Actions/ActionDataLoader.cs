using System;
using UnityEditor;

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
                    GetCachedList(typeof(ActionData));
                return actionList;
            }
        }

        private static ActionAttributeData[] actionAttributeList;
        public static ActionAttributeData[] ActionAttributeList
        {
            get
            {
                if (actionAttributeList == null)
                    GetCachedList(typeof(ActionAttributeData));
                return actionAttributeList;
            }
        }

        private static void GetCachedList(Type type)
        {
            string[] guids = AssetDatabase.FindAssets("t:"+type.Name);

            actionList = new ActionData[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                actionList[i] = (ActionData)AssetDatabase.LoadAssetAtPath(path, type);
            }
        }
    }
}
