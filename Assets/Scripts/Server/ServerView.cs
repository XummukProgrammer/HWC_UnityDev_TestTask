using UnityEngine;

public class ServerView : MonoBehaviour
{
    public NetworkServer NetworkServer {  get; private set; }
    public ServerController Controller { get; private set; }

    private void Awake()
    {
        NetworkServer = new();
        NetworkServer.OnInit();

        Controller = new(NetworkServer);
        Controller.OnInit();
    }

    private void OnDestroy()
    {
        NetworkServer.OnDeinit();
        NetworkServer = null;

        Controller.OnDeinit();
        Controller = null;
    }

    private void Update()
    {
        NetworkServer.OnUpdate();
        Controller.OnUpdate();
    }
}
