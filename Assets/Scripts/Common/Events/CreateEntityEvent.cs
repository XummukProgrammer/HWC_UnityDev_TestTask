using UnityEngine;

[System.Serializable]
public class CreateEntityEvent : Event
{
    public int EntityId;
    public Vector3 Position;
    public string PrefabId;
}
