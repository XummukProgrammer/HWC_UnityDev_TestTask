using Unity.Networking.Transport;
using UnityEngine;

public class NetworkServer
{
    public NetworkDriver Driver { get; private set; }

    public event System.Action<NetworkConnection> Accepted;

    public void OnInit()
    {
        Driver = NetworkDriver.Create();

        var endpoint = NetworkEndPoint.LoopbackIpv4;
        endpoint.Port = 3000;

        if (Driver.Bind(endpoint) != 0)
        {
            Debug.Log("Failed to bind to port 9000");
        }
        else
        {
            Driver.Listen();
        }
    }

    public void OnDeinit()
    {
        if (Driver.IsCreated)
        {
            Driver.Dispose();
        }
    }

    public void OnUpdate()
    {
        Driver.ScheduleUpdate().Complete();

        // Accept new connections
        NetworkConnection c;
        while ((c = Driver.Accept()) != default(NetworkConnection))
        {
            Accepted?.Invoke(c);
        }
    }

    public NetworkEvent.Type PopEventForConnection(NetworkConnection connection, out DataStreamReader stream)
    {
        return Driver.PopEventForConnection(connection, out stream);
    }
}
