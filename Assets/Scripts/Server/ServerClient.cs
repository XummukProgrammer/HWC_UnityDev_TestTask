using Unity.Networking.Transport;

public class ServerClient
{
    private NetworkDriver _driver;
    private NetworkEventableObject _networkEventableObject;

    public NetworkConnection Connection { get; private set; }

    public int Slot { get; set; }
    public bool IsCreate => Connection != null && Connection.IsCreated;

    public ServerClient(NetworkDriver driver)
    {
        _driver = driver;
    }

    public void Connect(NetworkConnection connection)
    {
        Connection = connection;

        _networkEventableObject = new(_driver, Connection);
    }

    public void Disconnect()
    {
        Slot = -1;
        Connection = default(NetworkConnection);

        _networkEventableObject = null;
    }

    public void PrintToChat(string message)
    {
        var @event = new PlayerChatEvent
        {
            Id = "PlayerChatEvent",
            Message = message
        };

        SendNetworkEvent(@event);
    }

    private void SendNetworkEvent(Event @event)
    {
        _networkEventableObject.Send(@event);
    }
}
