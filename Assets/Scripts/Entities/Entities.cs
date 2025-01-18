using System.Collections.Generic;
using UnityEngine;

public static class Entities
{
    private static EntitiesContainerView _containerView;
    private static List<ServerEntity> _serverEntities = new();

    public static IEnumerator<ServerEntity> ServerEntities = _serverEntities.GetEnumerator();

    public static void Init()
    {
        _containerView = GameObject.FindObjectOfType<EntitiesContainerView>();
    }

    public static void CreateClientEntity(int entityId, Vector3 position, string prefabId)
    {
        if (_containerView != null)
        {
            _containerView.Create(entityId, position, prefabId);
        }
    }

    public static void CreateServerEntity(int entityId, Vector3 position, string prefabId)
    {
        _serverEntities.Add(new ServerEntity
        {
            EntityId = entityId,
            Position = position,
            PrefabId = prefabId
        });
    }

    public static EntityView GetClientEntity(int entityId)
    {
        if (_containerView != null)
        {
            return _containerView.GetEntity(entityId);
        }
        return null;
    }

    public static ServerEntity GetServerEntity(int entityId)
    {
        foreach (var entity in _serverEntities)
        {
            if (entity.EntityId == entityId)
            {
                return entity;
            }
        }
        return null;
    }
}
