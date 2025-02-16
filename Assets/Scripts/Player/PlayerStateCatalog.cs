using System;
using UnityEngine;

namespace ProjectSecurity.Gameplay
{
	public static class PlayerStateCatalog
	{
		public static BaseState InstantiateState(Type stateType)
		{
			if (stateType != null && stateType.IsSubclassOf(typeof(BaseState)))
			{
				return Activator.CreateInstance(stateType) as BaseState;
			}
			Debug.LogFormat("Bad stateType {0}", new object[]
			{
				(stateType == null) ? "null" : stateType.FullName
			});
			return null;
		}
	}
}
