using UnityEngine;

public class ServerView : MonoBehaviour
{
    [SerializeField] private string _name;

    public Server Server { get; private set; }

    private void Awake()
    {
        var serverData = new ServerData
        {
            Name = _name
        };

        Server = new Server(serverData);
        Server.Run();
    }

    private void OnDestroy()
    {
        Server.Stop();
        Server = null;
    }
}
