using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class ServerView : MonoBehaviour
{
    private NetworkDriver _driver;
    private NativeList<NetworkConnection> _connections;

    private Events _events = new();
    private EventListener<PlayerConnectEvent> _playerConnectListener;

    private void Start()
    {
        _driver = NetworkDriver.Create();

        var endpoint = NetworkEndPoint.LoopbackIpv4;
        endpoint.Port = 3000;

        if (_driver.Bind(endpoint) != 0)
        {
            Debug.Log("Failed to bind to port 9000");
        }
        else
        {
            _driver.Listen();
        }

        _connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);

        _playerConnectListener = new EventListener<PlayerConnectEvent>
        {
            Accepted = (@event) =>
            {
                Debug.Log($"PLAYER CONNECT (MyId: {@event.MyId}, Name: {@event.Name})");
            }
        };
        _events.AddListener(_playerConnectListener);
    }

    private void OnDestroy()
    {
        if (_driver.IsCreated)
        {
            _driver.Dispose();
            _connections.Dispose();
        }

        _events.RemoveListener(_playerConnectListener);
        _playerConnectListener = null;
    }

    private void Update()
    {
        _driver.ScheduleUpdate().Complete();

        // Clean up connections
        for (int i = _connections.Length - 1; i >= 0; i--)
        {
            if (!_connections[i].IsCreated)
            {
                _connections.RemoveAtSwapBack(i);
            }
        }

        // Accept new connections
        NetworkConnection c;
        while ((c = _driver.Accept()) != default(NetworkConnection))
        {
            _connections.Add(c);
            Debug.Log("Accepted a connection");
        }

        DataStreamReader stream;
        for (int i = 0; i < _connections.Length; i++)
        {
            if (!_connections[i].IsCreated)
            {
                continue;
            }

            NetworkEvent.Type cmd;
            while ((cmd = _driver.PopEventForConnection(_connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Data)
                {
                    var jsonEvent = stream.ReadFixedString128();
                    //Debug.Log($"Event: {jsonEvent}");

                    _events.Send(jsonEvent.ToString());
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client disconnected from server");
                    _connections[i] = default(NetworkConnection);
                }
            }
        }
    }
}
