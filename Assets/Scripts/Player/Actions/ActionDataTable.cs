using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    [CreateAssetMenu(fileName = "ActionDataTable", menuName = "Scriptable Objects/Action Data Table")]
    public class ActionDataTable : ScriptableObject
    {
        public ActionData[] actionDatas;

        public ActionAttributeData[] actionAttributeDatas;
    }
}
