using System.Collections.Generic;

public class ClientAbilities
{
    public ServerClient Client { get; }
    public IEnumerator<Ability> Abilities => _abilities.GetEnumerator();

    private List<Ability> _abilities = new();
    private AbilitiesFeatures _features;

    public ClientAbilities(ServerClient client, AbilitiesFeatures features)
    {
        Client = client;

        _features = features;

        var container = _features.Features;
        while (container.MoveNext())
        {
            var feature = container.Current;
            _abilities.Add(new Ability(Client, feature.Id));
        }
    }

    public void Activate(string id)
    {
        var ability = GetAbility(id);
        var feature = _features.Get(id);

        if (ability != null && feature != null)
        {
            feature.OnActivate(Client);
        }
    }

    private Ability GetAbility(string id)
    {
        foreach (var ability in _abilities)
        {
            if (ability.Id == id)
            {
                return ability;
            }
        }
        return null;
    }
}
