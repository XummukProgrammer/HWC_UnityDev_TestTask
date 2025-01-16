using UnityEngine;

public class Client
{
    private ClientData _data;

    public Client(ClientData data)
    {
        _data = data;
    }

    public void Connect()
    {
        var @event = new PlayerConnectEvent
        {
            Id = "PlayerConnectEvent",
            MyId = _data.MyId,
            Name = _data.Name,
        };

        SendEvent(@event);
    }

    public void Disconnect()
    {
    }

    public void SendEvent(Event @event)
    {
        var jsonEvent = JsonUtility.ToJson(@event);
        _data.Server.Events.Send(jsonEvent);
    }
}
