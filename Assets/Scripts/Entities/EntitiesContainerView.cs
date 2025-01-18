using System.Collections.Generic;
using UnityEngine;

public class EntitiesContainerView : MonoBehaviour
{
    [SerializeField] private EntityView[] _prefabs;

    private List<EntityView> _entities = new();

    public void Create(int id, Vector3 position, string prefabId)
    {
        var prefab = GetPrefab(prefabId);
        var entity = Instantiate(prefab, gameObject.transform);
        entity.Id = id;
        entity.transform.position = position;
        _entities.Add(entity);
    }

    public EntityView GetEntity(int entityId)
    {
        foreach (var entity in _entities)
        {
            if (entity.Id == entityId)
            {
                return entity;
            }
        }
        return null;
    }

    private EntityView GetPrefab(string id)
    {
        foreach (var prefab in _prefabs)
        {
            if (prefab.name == id)
            {
                return prefab;
            }
        }
        return null;
    }
}
