using UnityEngine;

internal class PlayerConnectEventListener : IEventListener<PlayerConnectEvent>
{
    public void Accept(PlayerConnectEvent @event)
    {
        Debug.Log($"PLAYER CONNECT (MyId: {@event.MyId}, Name: {@event.Name})");
    }
}

public class Server
{
    private ServerData _data;
    private PlayerConnectEventListener _playerConnectEventListener;

    public Events Events { get; private set; } = new();

    public Server(ServerData data)
    {
        _data = data;
    }

    public void Run()
    {
        _playerConnectEventListener = new();
        Events.AddListener(_playerConnectEventListener);
    }

    public void Stop()
    {
        Events.RemoveListener(_playerConnectEventListener);
        _playerConnectEventListener = null;
    }
}
