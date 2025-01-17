public interface IAbilityFeature
{
    public string Id { get; }

    public void OnInit();

    public void OnActivate(ServerClient client);
    public void OnDeactivate(ServerClient client);
}
