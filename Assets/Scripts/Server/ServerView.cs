using UnityEngine;

public class ServerView : MonoBehaviour
{
    private NetworkServer _networkServer;
    private ServerController _controller;
    
    private void Start()
    {
        _networkServer = new();
        _networkServer.OnInit();

        _controller = new(_networkServer);
        _controller.OnInit();
    }

    private void OnDestroy()
    {
        _networkServer.OnDeinit();
        _networkServer = null;

        _controller.OnDeinit();
        _controller = null;
    }

    private void Update()
    {
        _networkServer.OnUpdate();
        _controller.OnUpdate();
    }
}
