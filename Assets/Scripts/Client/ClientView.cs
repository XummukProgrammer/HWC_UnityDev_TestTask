using UnityEngine;

public class ClientView : MonoBehaviour
{
    public NetworkClient NetworkClient {  get; private set; }
    public ClientController Controller { get; private set; }

    private void Awake()
    {
        NetworkClient = new();
        NetworkClient.OnInit();

        NetworkClient.Connected += OnConnected;
        NetworkClient.DataGetted += OnDataGetted;

        Controller = new(NetworkClient);
        Controller.OnInit();
    }

    private void OnDestroy()
    {
        NetworkClient.Connected -= OnConnected;
        NetworkClient.DataGetted -= OnDataGetted;

        NetworkClient.OnDeinit();
        NetworkClient = null;

        Controller.OnDeinit();
        Controller = null;
    }

    private void Update()
    {
        NetworkClient.OnUpdate();
    }

    private void OnConnected()
    {
        Controller.OnConnect();
    }

    private void OnDataGetted(string jsonEvent)
    {
        Controller.FireEvent(jsonEvent);
    }
}
