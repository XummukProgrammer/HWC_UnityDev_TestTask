using UnityEditor;

public class AttackingAbilityFeature : IAbilityFeature
{
    public string Id => typeof(AttackingAbilityFeature).Name;

    private ChatPlugin _chatPlugin;
    private AttackingAbilityModel _model;

    public void OnInit()
    {
        _chatPlugin = Plugins.Get<ChatPlugin>();

        _model = AssetDatabase.LoadAssetAtPath<AttackingAbilityModel>("Assets/Models/Abilities/Attacking.asset");
        var i = 0;
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