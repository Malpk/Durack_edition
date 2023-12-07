using UnityEngine;
using WebSocketSharp;
using System.Collections.Generic;

public class SocketServer : MonoBehaviour
{
    [SerializeField] private string _ip = "166.88.134.211"; //Айпи адрес сервера с бекендом
    [SerializeField] private string _port = "9954"; //Порт подключения


    private WebSocket _socket;
    private Dictionary<System.Action<string>, string> _requests = new Dictionary<System.Action<string>, string>();

    public event System.Action<MessageData> OnGetMessange;

    private void Awake()
    {
        _socket = new WebSocket("ws://" + _ip + ":" + _port);
        _socket.OnMessage += (sender, e) =>
        {
            string message = e.Data;
            GetAnswer(message);
        };
        _socket.OnError += (sender, e) =>
        {
            Debug.LogError("WebSocket: " + _socket.Url.ToString() + ", error: "
                + e.Exception + " : " + e.Message + " : " + sender);
        };
    }

    public void SendRequest(string answerKey, string messange, System.Action<string> action = null)
    {
        _socket.Connect();
        if (action != null)
        {
            if (!_requests.ContainsKey(action))
                _requests.Add(action, answerKey);
        }
        _socket.Send(messange);
    }
    public void SendRequest(string messange)
    {
        _socket.Connect();
        _socket.Send(messange);
    }

    protected void GetAnswer(string json)
    {
        if (!MainThreadDispatcher.IsRun)
            Debug.LogError("is't run MainThreadDispatcher");
        MainThreadDispatcher.RunOnMainThread(() =>
        {
            Debug.LogWarning("get: " + json);
            var messange = JsonUtility.FromJson<MessageData>(json);
            foreach (var reciver in GetRecivers(messange.eventType))
            {
                reciver.Invoke(messange.data);
                _requests.Remove(reciver);
            }
            OnGetMessange?.Invoke(messange);
        });
    }

    protected List<System.Action<string>> GetRecivers(string key)
    {
        var list = new List<System.Action<string>>();
        foreach (var request in _requests)
        {
            if(request.Value == key)
                list.Add(request.Key);
        }
        return list;
    }

}
