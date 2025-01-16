using Unity.Networking.Transport;
using UnityEngine;

public class ClientView : MonoBehaviour
{
    private NetworkDriver _driver;
    private NetworkConnection _connection;

    private ClientController _controller;

    private void Start()
    {
        _driver = NetworkDriver.Create();
        _connection = default(NetworkConnection);

        var endpoint = NetworkEndPoint.LoopbackIpv4;
        endpoint.Port = 3000;

        _connection = _driver.Connect(endpoint);

        _controller = new(_driver, _connection);
        _controller.OnInit();
    }

    private void OnDestroy()
    {
        _driver.Dispose();

        _controller.OnDeinit();
        _controller = null;
    }

    private void Update()
    {
        _driver.ScheduleUpdate().Complete();

        if (!_connection.IsCreated)
        {
            return;
        }
        
        DataStreamReader stream;
        NetworkEvent.Type cmd;
        while ((cmd = _connection.PopEvent(_driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                _controller.OnConnect();
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                var jsonEvent = stream.ReadFixedString128();
                _controller.SendEvent(jsonEvent.ToString());
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                _connection = default(NetworkConnection);
            }
        }
    }
}
