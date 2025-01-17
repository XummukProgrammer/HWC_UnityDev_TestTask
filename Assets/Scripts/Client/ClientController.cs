using UnityEngine;

public class ClientController
{
    private NetworkClient _networkClient;
    private NetworkEventableObject _networkEventableObject;

    private Events _events = new();
    private EventListener<PlayerChatEvent> _chatListener;

    public ClientController(NetworkClient networkClient)
    {
        _networkClient = networkClient;
    }

    public void OnInit()
    {
        _networkEventableObject = new(_networkClient.Driver, _networkClient.Connection);

        _chatListener = new EventListener<PlayerChatEvent>
        {
            Accepted = (@event) =>
            {
                Debug.Log(@event.Message);
            }
        };
        _events.AddListener(_chatListener);
    }

    public void OnDeinit()
    {
        _networkEventableObject = null;

        _events.RemoveListener(_chatListener);
        _chatListener = null;
    }

    public void OnConnect()
    {
    }

    public void PrintToChat(int[] slots, string message)
    {
        var @event = new PlayerChatEvent
        {
            Message = message,
            Slots = slots,
        };

        SendNetworkEvent(@event);
    }

    public void PrintToChat(int slot, string message)
    {
        PrintToChat(new int[] { slot }, message);
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

    public void FireEvent(string jsonEvent)
    {
        _events.Fire(jsonEvent.ToString());
    }

    private void SendNetworkEvent<T>(T @event) where T : Event
    {
        _networkEventableObject.Send(@event);
    }
}
