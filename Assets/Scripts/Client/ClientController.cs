public class ClientController
{
    private NetworkClient _networkClient;
    private NetworkEventableObject _networkEventableObject;

    public int Slot { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public int UID { get; set; }
    public Events Events { get; private set; } = new();

    public event System.Action Connected;

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
        Connected?.Invoke();
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
