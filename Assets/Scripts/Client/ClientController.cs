public class ClientController
{
    private NetworkClient _networkClient;
    private NetworkEventableObject _networkEventableObject;

    public Events Events { get; private set; } = new();

    public ClientController(NetworkClient networkClient)
    {
        _networkClient = networkClient;
    }

    public void OnInit()
    {
        _networkEventableObject = new(_networkClient.Driver, _networkClient.Connection);
    }

    public void OnDeinit()
    {
        _networkEventableObject = null;
    }

    public void OnConnect()
    {
    }

    public void FireEvent(string jsonEvent)
    {
        Events.Fire(jsonEvent.ToString());
    }

    public void SendNetworkEvent<T>(T @event) where T : Event
    {
        _networkEventableObject.Send(@event);
    }
}
