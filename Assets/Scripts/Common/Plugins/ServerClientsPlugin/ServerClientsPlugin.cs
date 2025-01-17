public class ServerClientsPlugin : Plugin
{
    public override string Name => "Server Clients";
    public override string Version => "1.0.0";
    public override string Author => "Xummuk97";

    public ServerClient GetClientBySlot(int slot)
    {
        if (ServerController != null && slot >= 0 && slot < 16)
        {
            return ServerController.Clients[slot];
        }
        return null;
    }

    public ServerClient GetClientByUserId(int userId)
    {
        if (ServerController != null)
        {
            foreach (var client in ServerController.Clients)
            {
                if (client.UserId == userId)
                {
                    return client;
                }
            }
        }
        return null;
    }

    public ServerClient GetClientByName(string name)
    {
        if (ServerController != null)
        {
            foreach (var client in ServerController.Clients)
            {
                if (client.Name == name)
                {
                    return client;
                }
            }
        }
        return null;
    }

    public ServerClient GetClientByUID(int UID)
    {
        if (ServerController != null)
        {
            foreach (var client in ServerController.Clients)
            {
                if (client.UID == UID)
                {
                    return client;
                }
            }
        }
        return null;
    }
}
