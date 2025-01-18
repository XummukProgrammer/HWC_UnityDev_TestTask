using UnityEngine;

public class SpawnsContainerView : MonoBehaviour
{
    [SerializeField] private SpawnEntityView[] _spawns;

    public int Count => _spawns.Length;

    public SpawnEntityView Get(int index)
    {
        if (index >= 0 && index < _spawns.Length)
        {
            return _spawns[index];
        }
        return null;
    }
}
