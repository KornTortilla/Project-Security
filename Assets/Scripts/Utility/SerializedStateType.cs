using System;
using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    [Serializable]
    public struct SerializableStateType
    {
        [SerializeField] private string typeName;

        private Type stateType;

        public Type StateType
        {
            get
            {
                if (stateType == null)
                    CacheStateType();
                return stateType;
            }
        }

        private void CacheStateType()
        {
            if (typeName == null) return;

            Type type = Type.GetType("ProjectSecurity.Gameplay." + typeName);
            this.stateType = ((type != null && type.IsSubclassOf(typeof(BaseState))) ? type : null);
        }
    }
}
