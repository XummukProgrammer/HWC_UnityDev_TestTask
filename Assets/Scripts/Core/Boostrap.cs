using UnityEngine;

public class Boostrap : MonoBehaviour
{
    private void Awake()
    {
        EventsActions.Init();
    }

    private void Start()
    {
        var clientView = FindObjectOfType<ClientView>();
        if (clientView != null)
        {
            Plugins.ClientController = clientView.Controller;
        }

        var serverView = FindObjectOfType<ServerView>();
        if (serverView != null)
        {
            Plugins.ServerController = serverView.Controller;
        }

        Plugins.Add(new AuthPlugin());
        Plugins.Add(new ServerClientsPlugin());
        Plugins.Add(new ChatPlugin());
        Plugins.Add(new ControllerPlugin());

        if (Plugins.ServerController != null)
        {
            Plugins.Add(new AbilitiesPlugin());
        }

        Plugins.OnAllPluginsLoaded();
    }

    private void Update()
    {
        Plugins.OnUpdate();
    }
}
