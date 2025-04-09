using System;
using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public static class ContentLoader
    {
        private static ActionDataTable actionDataTable;
        public static ActionData[] ActionList
        {
            get
            {
                if (actionDataTable == null)
                    GetActionDataTable();
                return actionDataTable.actionDatas;
            }
        }

        public static ActionAttributeData[] ActionAttributeList
        {
            get
            {
                if (actionDataTable == null)
                    GetActionDataTable();
                return actionDataTable.actionAttributeDatas;
            }
        }

        private static void GetActionDataTable()
        {
            actionDataTable = Resources.Load<ActionDataTable>("ActionDataTable");
        }

        /*
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
        */
    }
}
