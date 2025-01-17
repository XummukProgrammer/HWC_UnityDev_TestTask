using UnityEngine;

public class ControllerPlugin : Plugin
{
    public override string Name => "Controller";
    public override string Version => "1.0.0";
    public override string Author => "Xummuk97";

    public event System.Action<ServerClient, ClientButton> ButtonPressed;

    private ServerClientsPlugin _serverClientsPlugin;

    private EventListener<PlayerButtonPressedEvent> _serverButtonPressedListener;

    public override void OnLoad()
    {
        base.OnLoad();

        if (ServerController != null)
        {
            _serverButtonPressedListener = new EventListener<PlayerButtonPressedEvent>
            {
                Accepted = OnPlayerButtonPressedHandler
            };
            ServerController.Events.AddListener(_serverButtonPressedListener);
        }
    }

    public override void OnUnload()
    {
        base.OnUnload();

        if (ServerController != null)
        {
            ServerController.Events.RemoveListener(_serverButtonPressedListener);
            _serverButtonPressedListener = null;
        }
    }

    public override void OnAllPluginsLoaded()
    {
        base.OnAllPluginsLoaded();

        _serverClientsPlugin = Plugins.Get<ServerClientsPlugin>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (Input.GetKeyDown(KeyCode.F1))
        {
            FireButton(ClientButton.Ability1);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            FireButton(ClientButton.Ability2);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            FireButton(ClientButton.Ability3);
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            FireButton(ClientButton.Ability4);
        }
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            FireButton(ClientButton.Ability5);
        }
    }

    private void FireButton(ClientButton button)
    {
        ClientController.SendNetworkEvent(new PlayerButtonPressedEvent
        {
            UserId = ClientController.UserId,
            Button = button
        });
    }

    private void OnPlayerButtonPressedHandler(PlayerButtonPressedEvent @event)
    {
        if (_serverClientsPlugin == null)
        {
            return;
        }

        var client = _serverClientsPlugin.GetClientByUserId(@event.UserId);
        if (client != null)
        {
            ButtonPressed?.Invoke(client, @event.Button);
        }
    }
}
