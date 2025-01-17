using UnityEngine;

public class ClientView : MonoBehaviour
{
    private NetworkClient _networkClient;
    private ClientController _controller;

    private void Start()
    {
        _networkClient = new();
        _networkClient.OnInit();

        _networkClient.Connected += OnConnected;
        _networkClient.DataGetted += OnDataGetted;

        _controller = new(_networkClient);
        _controller.OnInit();
    }

    private void OnDestroy()
    {
        _networkClient.Connected -= OnConnected;
        _networkClient.DataGetted -= OnDataGetted;

        _networkClient.OnDeinit();
        _networkClient = null;

        _controller.OnDeinit();
        _controller = null;
    }

    private void Update()
    {
        _networkClient.OnUpdate();
    }

    private void OnConnected()
    {
        _controller.OnConnect();
    }

    private void OnDataGetted(string jsonEvent)
    {
        _controller.FireEvent(jsonEvent);
    }
}
