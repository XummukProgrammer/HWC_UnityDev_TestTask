using Unity.Networking.Transport;
using UnityEngine;

public class ClientController
{
    private NetworkDriver _driver;
    private NetworkConnection _connection;

    private NetworkEventableObject _networkEventableObject;

    private Events _events = new();
    private EventListener<TestEvent> _testListener;

    public ClientController(NetworkDriver driver, NetworkConnection connection)
    {
        _driver = driver;
        _connection = connection;
    }

    public void OnInit()
    {
        _networkEventableObject = new(_driver, _connection);

        _testListener = new EventListener<TestEvent>
        {
            Accepted = (@event) =>
            {
                Debug.Log($"MESSAGE: {@event.Message}");
            }
        };
        _events.AddListener(_testListener);
    }

    public void OnDeinit()
    {
        _networkEventableObject = null;

        _events.RemoveListener(_testListener);
        _testListener = null;
    }

    public void OnConnect()
    {
        var @event = new PlayerConnectEvent
        {
            Id = "PlayerConnectEvent",
            Name = "Xummuk97",
            MyId = 1234
        };

        _networkEventableObject.Send(@event);
    }

    public void SendEvent(string jsonEvent)
    {
        _events.Send(jsonEvent.ToString());
    }
}
