using UnityEngine;

public class ChatPlugin : Plugin
{
    public override string Name => "Chat";
    public override string Version => "1.0.0";
    public override string Author => "Xummuk97";

    private EventListener<PlayerChatEvent> _clientChatListener;
    private EventListener<PlayerChatEvent> _serverChatListener;

    public override void OnLoad()
    {
        base.OnLoad();

        if (ClientController != null)
        {
            _clientChatListener = new EventListener<PlayerChatEvent>
            {
                Accepted = OnClientChatHandler
            };
            ClientController.Events.AddListener(_clientChatListener);
        }

        if (ServerController != null)
        {
            _serverChatListener = new EventListener<PlayerChatEvent>
            {
                Accepted = OnServerChatHandler
            };
            ServerController.Events.AddListener(_serverChatListener);
        }
    }

    public override void OnUnload()
    {
        base.OnUnload();

        if (ClientController != null)
        {
            ClientController.Events.RemoveListener(_clientChatListener);
            _clientChatListener = null;
        }

        if (ServerController != null)
        {
            ServerController.Events.RemoveListener(_serverChatListener);
            _serverChatListener = null;
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (Input.GetKeyDown(KeyCode.A))
        {
            PrintToChatAll("Hello, World!");
        }
    }

    public void PrintToChat(int[] slots, string message)
    {
        if (ClientController != null)
        {
            ClientController.SendNetworkEvent(new PlayerChatEvent
            {
                Slots = slots,
                Message = message
            });
        }
    }

    public void PrintToChat(int slot, string message)
    {
        int[] slots = new int[1] { slot };
        PrintToChat(slots, message);
    }

    public void PrintToChatAll(string message)
    {
        int[] slots = new int[16];
        for (int i = 0; i < 16; i++)
        {
            slots[i] = i;
        }

        PrintToChat(slots, message);
    }

    private void OnClientChatHandler(PlayerChatEvent @event)
    {
        Debug.Log(@event.Message);
    }

    private void OnServerChatHandler(PlayerChatEvent @event)
    {
        foreach (var slot in @event.Slots)
        {
            ServerController.Clients[slot].SendNetworkEvent(new PlayerChatEvent
            {
                Message = @event.Message
            });
        }
    }
}
