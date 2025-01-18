using UnityEngine;

[System.Serializable]
public class PlayerCreateEntityEvent : Event
{
    public int EntityId;
    public Vector3 Position;
    public string PrefabId;
}
