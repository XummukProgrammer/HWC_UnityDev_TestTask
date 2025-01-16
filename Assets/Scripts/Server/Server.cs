using UnityEngine;

public class Server
{
    private ServerData _data;

    public Server(ServerData data)
    {
        _data = data;
    }

    public void Run()
    {
    }

    public void Stop()
    {
    }

    public void SendEvent(string jsonEvent)
    {
        var @event = JsonUtility.FromJson<Event>(jsonEvent);
        if (@event.Id == "PlayerConnectEvent")
        {
            var ev = JsonUtility.FromJson<PlayerConnectEvent>(jsonEvent);
        }
    }
}
