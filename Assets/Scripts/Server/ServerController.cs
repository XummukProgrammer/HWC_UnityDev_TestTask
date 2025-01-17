using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class ServerController
{
    private NetworkServer _networkServer;
    private List<ServerClient> _clients;
    private int _connectedClients;
    private Dictionary<int, bool> _openSlots;

    private Events _events = new();
    private EventListener<PlayerChatEvent> _chatListener;

    public ServerController(NetworkServer networkServer)
    {
        _networkServer = networkServer;
    }

    public void OnInit()
    {
        _connectedClients = 0;

        _clients = new();
        _openSlots = new();
        for (int i = 0; i < 16; i++)
        {
            _clients.Add(new ServerClient(_networkServer.Driver));
            _openSlots[i] = true;
        }

        _networkServer.Accepted += OnAccepted;

        _chatListener = new EventListener<PlayerChatEvent>
        {
            Accepted = (@event) =>
            {
                foreach (var slot in @event.Slots)
                {
                    if (_clients[slot].IsCreate)
                    {
                        _clients[slot].PrintToChat(@event.Message);
                    }
                }
            }
        };
        _events.AddListener(_chatListener);
    }

    public void OnDeinit()
    {
        _networkServer.Accepted -= OnAccepted;

        _events.RemoveListener(_chatListener);
        _chatListener = null;
    }

    public void OnUpdate()
    {
        DataStreamReader stream;
        foreach (var client in _clients)
        {
            if (!client.IsCreate)
            {
                continue;
            }

            NetworkEvent.Type cmd;
            while ((cmd = _networkServer.PopEventForConnection(client.Connection, out stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Data)
                {
                    var jsonEvent = stream.ReadFixedString128();
                    _events.Fire(jsonEvent.ToString());
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client disconnected from server");
                    SetSlotValue(client.Slot, true);
                    client.Disconnect();
                }
            }
        }
    }

    private (int, bool) GetOpenSlot()
    {
        foreach (var slot in _openSlots)
        {
            if (slot.Value)
            {
                return (slot.Key, true);
            }
        }
        return (-1, false);
    }

    private void SetSlotValue(int slot, bool value)
    {
        _openSlots[slot] = value;
    }

    private void OnAccepted(NetworkConnection connection)
    {
        var (slot, hasSlot) = GetOpenSlot();
        if (hasSlot)
        {
            _clients[_connectedClients].Slot = slot;
            _clients[_connectedClients].Connect(connection);
            _connectedClients++;
            SetSlotValue(slot, false);
            Debug.Log("Accepted a connection");
        }
    }
}
