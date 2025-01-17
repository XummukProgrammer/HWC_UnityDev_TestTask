using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class ServerController
{
    private NetworkServer _networkServer;
    private int _connectedClients;
    private Dictionary<int, bool> _openSlots;

    private EventListener<PlayerChatEvent> _chatListener;

    public List<ServerClient> Clients { get; private set; }
    public Events Events { get; private set; } = new();

    public ServerController(NetworkServer networkServer)
    {
        _networkServer = networkServer;
    }

    public void OnInit()
    {
        _connectedClients = 0;

        Clients = new();
        _openSlots = new();
        for (int i = 0; i < 16; i++)
        {
            Clients.Add(new ServerClient(_networkServer.Driver));
            _openSlots[i] = true;
        }

        _networkServer.Accepted += OnAccepted;
    }

    public void OnDeinit()
    {
        _networkServer.Accepted -= OnAccepted;
    }

    public void OnUpdate()
    {
        DataStreamReader stream;
        foreach (var client in Clients)
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
                    Events.Fire(jsonEvent.ToString());
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
            Clients[slot].Slot = slot;
            Clients[slot].Connect(connection);
            _connectedClients++;
            SetSlotValue(slot, false);
            Debug.Log("Accepted a connection");
        }
    }
}
