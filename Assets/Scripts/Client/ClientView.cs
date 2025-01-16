using UnityEngine;

public class ClientView : MonoBehaviour
{
    [SerializeField] private ServerView _serverView;
    [SerializeField] private string _name;
    [SerializeField] private int _myId;

    private Client _client;

    private void Start()
    {
        var clientData = new ClientData
        {
            Server = _serverView.Server,
            MyId = _myId,
            Name = _name
        };

        _client = new Client(clientData);
        _client.Connect();
    }

    private void OnDestroy()
    {
        _client.Disconnect();
        _client = null;
    }
}
