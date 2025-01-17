public class AttackingAbilityFeature : IAbilityFeature
{
    public string Id => typeof(AttackingAbilityFeature).Name;

    private ChatPlugin _chatPlugin;

    public void OnInit()
    {
        _chatPlugin = Plugins.Get<ChatPlugin>();
    }

    public void OnActivate(ServerClient client)
    {
        if (_chatPlugin != null)
        {
            _chatPlugin.PrintToChat(client.Slot, $"Activate: {Id}");
        }
    }

    public void OnDeactivate(ServerClient client)
    {
    }
}
