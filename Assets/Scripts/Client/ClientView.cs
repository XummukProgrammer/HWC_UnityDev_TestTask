using Unity.Networking.Transport;
using UnityEngine;

public class ClientView : MonoBehaviour
{
    private NetworkDriver _driver;
    private NetworkConnection _connection;
    private bool _isDone;

    private NetworkEventableObject _networkEventableObject;

    private void Start()
    {
        _driver = NetworkDriver.Create();
        _connection = default(NetworkConnection);

        var endpoint = NetworkEndPoint.LoopbackIpv4;
        endpoint.Port = 3000;

        _connection = _driver.Connect(endpoint);

        _networkEventableObject = new(_driver, _connection);
    }

    private void OnDestroy()
    {
        _driver.Dispose();

        _networkEventableObject = null;
    }

    private void Update()
    {
        _driver.ScheduleUpdate().Complete();

        if (!_connection.IsCreated)
        {
            if (!_isDone)
            {
                Debug.Log("Something went wrong during connect");
            }
            return;
        }
        
        DataStreamReader stream;
        NetworkEvent.Type cmd;
        while ((cmd = _connection.PopEvent(_driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                OnConnect();
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                /*uint value = stream.ReadUInt();
                Debug.Log("Got the value = " + value + " back from the server");
                _isDone = true;
                _connection.Disconnect(_driver);
                _connection = default(NetworkConnection);*/
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                OnDisconnect();

                _connection = default(NetworkConnection);
            }
        }
    }

    private void OnConnect()
    {
        var @event = new PlayerConnectEvent
        {
            Id = "PlayerConnectEvent",
            Name = "Xummuk97",
            MyId = 1234
        };

        _networkEventableObject.Send(@event);
    }

    private void OnDisconnect()
    {
    }
}
