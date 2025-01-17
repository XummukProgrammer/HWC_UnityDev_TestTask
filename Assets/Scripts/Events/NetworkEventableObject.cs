using Unity.Networking.Transport;
using UnityEngine;

public class NetworkEventableObject
{
    private NetworkDriver _driver;
    private NetworkConnection _connection;

    public NetworkEventableObject(NetworkDriver driver, NetworkConnection connection)
    {
        _driver = driver;
        _connection = connection;
    }

    public void Send<T>(T @event) where T : Event
    {
        @event.Id = typeof(T).Name;

        var jsonEvent = JsonUtility.ToJson(@event);

        _driver.BeginSend(_connection, out var writer);
        writer.WriteFixedString128(jsonEvent);
        _driver.EndSend(writer);
    }
}
