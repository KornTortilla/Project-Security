using System;
using UnityEditor;

namespace ProjectSecurity.Gameplay
{
    public static class ContentLoader
    {
        private static ActionData[] actionList;
        public static ActionData[] ActionList
        {
            get
            {
                if (actionList == null)
                    GetActionCachedList();
                return actionList;
            }
        }

        private static ActionAttributeData[] actionAttributeList;
        public static ActionAttributeData[] ActionAttributeList
        {
            get
            {
                if (actionAttributeList == null)
                    GetAttributeCachedList();
                return actionAttributeList;
            }
        }

        private static void GetActionCachedList()
        {
            string[] guids = AssetDatabase.FindAssets("t:"+typeof(ActionData).ToString());

            actionList = new ActionData[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                actionList[i] = (ActionData)AssetDatabase.LoadAssetAtPath(path, typeof(ActionData));
            }
        }

        private static void GetAttributeCachedList()
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(ActionAttributeData).ToString());

            actionAttributeList = new ActionAttributeData[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                actionAttributeList[i] = (ActionAttributeData)AssetDatabase.LoadAssetAtPath(path, typeof(ActionAttributeData));
            }
        }
    }
}
