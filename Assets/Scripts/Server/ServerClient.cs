using Unity.Networking.Transport;

public class ServerClient
{
    private NetworkDriver _driver;

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
    }

    public void Disconnect()
    {
        Slot = -1;
        Connection = default(NetworkConnection);
    }
}
