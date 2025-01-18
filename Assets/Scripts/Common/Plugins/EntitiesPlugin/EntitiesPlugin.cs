using UnityEngine;

public class EntitiesPlugin : Plugin
{
    public override string Name => "Entities";
    public override string Version => "1.0.0";
    public override string Author => "Xummuk97";

    private int _entityId;

    private AuthPlugin _authPlugin;

    private EventListener<PlayerCreateEntityEvent> _playerCreateEntityListener;
    private EventListener<PlayerPullEntitiesEvent> _playerPullEntitiesListener;
    private EventListener<CreateEntityEvent> _createEntityListener;
    private EventListener<UpdateEntityPositionEvent> _serverUpdateEntityPositionListener;
    private EventListener<UpdateEntityPositionEvent> _clientUpdateEntityPositionListener;

    public event System.Action<int> Created;

    public override void OnLoad()
    {
        base.OnLoad();

        if (ServerController != null)
        {
            _playerCreateEntityListener = new EventListener<PlayerCreateEntityEvent>
            {
                Accepted = OnPlayerCreateEntityHandler
            };
            ServerController.Events.AddListener(_playerCreateEntityListener);

            _serverUpdateEntityPositionListener = new EventListener<UpdateEntityPositionEvent>
            {
                Accepted = OnUpdateServerEntityPositionHandler
            };
            ServerController.Events.AddListener(_serverUpdateEntityPositionListener);

            _playerPullEntitiesListener = new EventListener<PlayerPullEntitiesEvent>
            {
                Accepted = OnPlayerPullEntitiesHandler
            };
            ServerController.Events.AddListener(_playerPullEntitiesListener);
        }

        if (ClientController != null)
        {
            _createEntityListener = new EventListener<CreateEntityEvent>
            {
                Accepted = OnCreateEntityHandler
            };
            ClientController.Events.AddListener(_createEntityListener);

            _clientUpdateEntityPositionListener = new EventListener<UpdateEntityPositionEvent>
            {
                Accepted = OnUpdateClientEntityPositionHandler
            };
            ClientController.Events.AddListener(_clientUpdateEntityPositionListener);
        }
    }

    public override void OnUnload()
    {
        base.OnUnload();

        if (ServerController != null)
        {
            ServerController.Events.RemoveListener(_playerCreateEntityListener);
            _playerCreateEntityListener = null;

            ServerController.Events.RemoveListener(_serverUpdateEntityPositionListener);
            _serverUpdateEntityPositionListener = null;

            ClientController.Events.RemoveListener(_clientUpdateEntityPositionListener);
            _clientUpdateEntityPositionListener = null;

            ServerController.Events.RemoveListener(_playerPullEntitiesListener);
            _playerPullEntitiesListener = null;
        }

        if (ClientController != null)
        {
            ClientController.Events.RemoveListener(_createEntityListener);
            _createEntityListener = null;
        }

        if (_authPlugin != null)
        {
            _authPlugin.ClientFullConnected -= OnClientFullConnectedHandler;
        }
    }

    public override void OnAllPluginsLoaded()
    {
        base.OnAllPluginsLoaded();

        _authPlugin = Plugins.Get<AuthPlugin>();
        if (_authPlugin != null)
        {
            _authPlugin.ClientFullConnected += OnClientFullConnectedHandler;
        }
    }

    public void CreateClientEntity(Vector3 position, string prefabId)
    {
        if (ClientController != null)
        {
            ClientController.SendNetworkEvent(new PlayerCreateEntityEvent
            {
                EntityId = _entityId,
                Position = position,
                PrefabId = prefabId
            });

            _entityId++;
        }
    }

    public void UpdateEntityPosition(int entityId, Vector3 position)
    {
        if (ClientController != null)
        {
            ClientController.SendNetworkEvent(new UpdateEntityPositionEvent
            {
                EntityId = entityId,
                Position = position
            });
        }
    }

    private void OnPlayerCreateEntityHandler(PlayerCreateEntityEvent @event)
    {
        Entities.CreateServerEntity(@event.EntityId, @event.Position, @event.PrefabId);

        foreach (var client in ServerController.Clients)
        {
            if (!client.IsCreate)
            {
                continue;
            }

            client.SendNetworkEvent(new CreateEntityEvent
            {
                EntityId = @event.EntityId,
                Position = @event.Position,
                PrefabId = @event.PrefabId
            });
        }
    }

    private void OnCreateEntityHandler(CreateEntityEvent @event)
    {
        Entities.CreateClientEntity(@event.EntityId, @event.Position, @event.PrefabId);

        Created?.Invoke(@event.EntityId);
    }

    private void OnUpdateServerEntityPositionHandler(UpdateEntityPositionEvent @event)
    {
        var entity = Entities.GetServerEntity(@event.EntityId);
        if (entity != null)
        {
            entity.Position = @event.Position;
        }

        foreach (var client in ServerController.Clients)
        {
            if (!client.IsCreate)
            {
                continue;
            }

            client.SendNetworkEvent(new UpdateEntityPositionEvent
            {
                EntityId = @event.EntityId,
                Position = @event.Position,
            });
        }
    }

    private void OnUpdateClientEntityPositionHandler(UpdateEntityPositionEvent @event)
    {
        var entity = Entities.GetClientEntity(@event.EntityId);
        if (entity != null)
        {
            entity.transform.position = @event.Position;
        }
    }

    private void OnClientFullConnectedHandler(ServerClient client)
    {
        client.SendNetworkEvent(new PlayerPullEntitiesEvent
        {
            UserId = client.UserId
        });

        CreateClientEntity(Vector3.zero, "ClientEntity");
    }

    private void OnPlayerPullEntitiesHandler(PlayerPullEntitiesEvent @event)
    {
        foreach (var client in ServerController.Clients)
        {
            if (!client.IsCreate || client.UserId != @event.UserId)
            {
                continue;
            }

            var entities = Entities.ServerEntities;
            while (entities.MoveNext())
            {
                var entity = entities.Current;
                client.SendNetworkEvent(new CreateEntityEvent
                {
                    EntityId = entity.EntityId,
                    Position = entity.Position,
                    PrefabId = entity.PrefabId
                });
            }    
            return;
        }
    }
}
