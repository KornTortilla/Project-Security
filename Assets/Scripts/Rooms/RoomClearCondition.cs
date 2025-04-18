using System;
using UnityEngine;

public class RoomClearCondition : MonoBehaviour
{
    public DoorBehavior door;

    public int EnemyCount { get; private set;} = 0;

    private void Start() {
        EnemyCount = GetComponentsInChildren<EnemyHealth>().Length;
        if (EnemyCount <= 0) {
            ProcessRoomClear();
        }
    }
    
    public void DecreaseEnemyCount() {
        EnemyCount--;
        if (EnemyCount <= 0) {
            ProcessRoomClear();
        }
    }

    private void ProcessRoomClear()
    {
        if (door) door.EnableOpenDoorCollider();
        Debug.Log("Room is clear");
    }
}
