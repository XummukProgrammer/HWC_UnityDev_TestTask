using UnityEngine;

public class ClientController
{
    private NetworkClient _networkClient;
    private NetworkEventableObject _networkEventableObject;

    private Events _events = new();
    private EventListener<TestEvent> _testListener;

    public ClientController(NetworkClient networkClient)
    {
        _networkClient = networkClient;
    }

    public void OnInit()
    {
        _networkEventableObject = new(_networkClient.Driver, _networkClient.Connection);

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
