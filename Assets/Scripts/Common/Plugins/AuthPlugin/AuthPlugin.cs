using UnityEngine;

public class AuthPlugin : Plugin
{
    public override string Name => "Auth";
    public override string Version => "1.0.0";
    public override string Author => "Xummuk97";

    private ServerClientsPlugin _serverClientsPlugin;

    private EventListener<PlayerAcceptedEvent> _clientAcceptedListener;
    private EventListener<PlayerFullConnectedEvent> _serverFullConnectedListener;

    public event System.Action<ServerClient> ClientFullConnected;
    public event System.Action ClientAccepted;

    public override void OnLoad()
    {
        base.OnLoad();

        if (ClientController != null)
        {
            ClientController.Name = "Xummuk97";
            ClientController.UID = 12345;

            _clientAcceptedListener = new EventListener<PlayerAcceptedEvent>
            {
                Accepted = OnClientAcceptedHandler
            };
            ClientController.Events.AddListener(_clientAcceptedListener);
        }

        if (ServerController != null)
        {
            _serverFullConnectedListener = new EventListener<PlayerFullConnectedEvent>
            {
                Accepted = OnPlayerFullConnectedHandler
            };
            ServerController.Events.AddListener(_serverFullConnectedListener);
        }
    }

    public override void OnUnload()
    {
        base.OnUnload();

        if (ClientController != null)
        {
            ClientController.Events.RemoveListener(_clientAcceptedListener);
            _clientAcceptedListener = null;
        }

        if (ServerController != null)
        {
            ServerController.Events.RemoveListener(_serverFullConnectedListener);
            _serverFullConnectedListener = null;
        }
    }

    public override void OnAllPluginsLoaded()
    {
        base.OnAllPluginsLoaded();

        _serverClientsPlugin = Plugins.Get<ServerClientsPlugin>();
    }

    private void OnClientAcceptedHandler(PlayerAcceptedEvent @event)
    {
        ClientController.Slot = @event.Slot;
        ClientController.UserId = @event.UserId;

        ClientController.SendNetworkEvent(new PlayerFullConnectedEvent
        {
            Name = ClientController.Name,
            UID = ClientController.UID,
            UserId = @event.UserId,
            Slot = @event.Slot
        });

        ClientAccepted?.Invoke();
    }

    private void OnPlayerFullConnectedHandler(PlayerFullConnectedEvent @event)
    {
        if (_serverClientsPlugin == null)
        {
            return;
        }

        var client = _serverClientsPlugin.GetClientByUserId(@event.UserId);
        if (client != null)
        {
            client.Name = @event.Name;
            client.UID = @event.UID;

            ClientFullConnected?.Invoke(client);
        }
    }
}
