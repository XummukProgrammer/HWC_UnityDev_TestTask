using System.Collections.Generic;

public class AbilitiesPlugin : Plugin
{
    public override string Name => "Abilities";
    public override string Version => "1.0.0";
    public override string Author => "Xummuk97";

    private List<ClientAbilities> _clientsAbilities = new();
    private AbilitiesFeatures _features = new();

    private AuthPlugin _authPlugin;
    private ControllerPlugin _controllerPlugin;

    public override void OnLoad()
    {
        base.OnLoad();

        _features.Add<AttackingAbilityFeature>();
    }

    public override void OnUnload()
    {
        base.OnUnload();

        if (_authPlugin != null)
        {
            _authPlugin.ClientFullConnected -= OnClientFullConnected;
        }
    }

    public override void OnAllPluginsLoaded()
    {
        base.OnAllPluginsLoaded();

        _authPlugin = Plugins.Get<AuthPlugin>();
        if (_authPlugin != null)
        {
            _authPlugin.ClientFullConnected += OnClientFullConnected;
        }

        _controllerPlugin = Plugins.Get<ControllerPlugin>();
        if (_controllerPlugin != null)
        {
            _controllerPlugin.ButtonPressed += OnButtonPressed;
        }
    }

    public ClientAbilities GetClientAbilities(ServerClient serverClient)
    {
        foreach (var clientAbilities in _clientsAbilities)
        {
            if (clientAbilities.Client == serverClient)
            {
                return clientAbilities;
            }
        }
        return null;
    }

    private void OnClientFullConnected(ServerClient client)
    {
        _clientsAbilities.Add(new ClientAbilities(client, _features));
    }

    private void OnButtonPressed(ServerClient client, ClientButton button)
    {
        var clientAbilities = GetClientAbilities(client);
        if (clientAbilities != null)
        {
            clientAbilities.Activate(GetAbilityIdByButton(button));
        }
    }

    private string GetAbilityIdByButton(ClientButton button)
    {
        switch (button)
        {
            case ClientButton.Ability1:
                return typeof(AttackingAbilityFeature).Name;
        }
        return null;
    }
}
