using Unity.Networking.Transport;
using UnityEngine;

public class NetworkClient
{
    public event System.Action Connected;
    public event System.Action<string> DataGetted;

    public NetworkDriver Driver { get; private set; }
    public NetworkConnection Connection { get; private set; }

    public void OnInit()
    {
        Driver = NetworkDriver.Create();
        Connection = default(NetworkConnection);

        var endpoint = NetworkEndPoint.LoopbackIpv4;
        endpoint.Port = 3000;

        Connection = Driver.Connect(endpoint);
    }

    public void OnDeinit()
    {
        Driver.Dispose();
    }

    public void OnUpdate()
    {
        Driver.ScheduleUpdate().Complete();

        if (!Connection.IsCreated)
        {
            return;
        }

        DataStreamReader stream;
        NetworkEvent.Type cmd;
        while ((cmd = Connection.PopEvent(Driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                Connected?.Invoke();
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                var jsonEvent = stream.ReadFixedString128();
                DataGetted?.Invoke(jsonEvent.ToString());
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Connection = default(NetworkConnection);
            }
        }
    }
}
