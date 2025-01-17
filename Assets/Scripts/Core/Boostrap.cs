using UnityEngine;

public class Boostrap : MonoBehaviour
{
    private void Start()
    {
        EventsActions.Init();

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

        Plugins.Add(new ChatPlugin());
        Plugins.OnAllPluginsLoaded();
    }

    private void Update()
    {
        Plugins.OnUpdate();
    }
}
