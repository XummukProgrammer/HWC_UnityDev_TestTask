using UnityEngine;

[System.Serializable]
public class UpdateEntityPositionEvent : Event
{
    public int EntityId;
    public Vector3 Position;
}
