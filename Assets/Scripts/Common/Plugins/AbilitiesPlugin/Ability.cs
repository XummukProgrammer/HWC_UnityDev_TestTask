public class Ability
{
    public ServerClient Client { get; }
    public string Id { get; }

    public Ability(ServerClient client, string id)
    {
        Client = client;
        Id = id;
    }
}
