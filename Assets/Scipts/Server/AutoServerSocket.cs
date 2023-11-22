using UnityEngine;
using UnityEngine.Events;
using WebSocketSharp;

public class AutoServerSocket : MonoBehaviour
{
    [SerializeField] private string _ip = "166.88.134.211"; //���� ����� ������� � ��������
    [SerializeField] private string _port = "9954"; //���� �����������
    [Header("Reference")]
    [SerializeField] private AutoMenu _auto;  //���� �����������
    [SerializeField] private RegistrationMenu _registration;  //���� �����������
    [Header("Events")]
    [SerializeField] private UnityEvent<MessageData> _onGetMessange;

    private WebSocket _socket;

    private void Awake()
    {
        _auto.OnAuto += SendMessange;
        _registration.OnRegistration += SendMessange;
    }

    private void OnDestroy()
    {
        _auto.OnAuto -= SendMessange;
        _registration.OnRegistration -= SendMessange;
    }

    private void Start()
    {
        _socket = new WebSocket("ws://" + _ip + ":" + _port);
        _socket.OnMessage += (sender, e) =>
        {
            string message = e.Data;
            GetMessange(message);
        };
        _socket.OnError += (sender, e) =>
        {
            Debug.LogError("WebSocket: " + _socket.Url.ToString() + ", error: "
                + e.Exception + " : " + e.Message + " : " + sender);
        };
        _socket.Connect();
    }

    private void SendMessange(string key, string data)
    {
        var messange = JsonUtility.ToJson(new MessageData(key, data));
        _socket.Send(messange);
    }

    private void GetMessange(string message)
    {
        MainThreadDispatcher.RunOnMainThread(() =>
        {
            var data = JsonUtility.FromJson<MessageData>(message);
            _onGetMessange.Invoke(data);
        });
    }
}
