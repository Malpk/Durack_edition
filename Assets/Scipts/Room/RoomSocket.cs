using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using Server;

public class RoomSocket : MonoBehaviour
{
    [SerializeField] private SocketServer _socket;

    private Dictionary<string, System.Action<string>> _actions = 
        new Dictionary<string, System.Action<string>>();

    public event System.Action<string> OnJoinPlayer;
    public event System.Action<string> OnLeavePlayer;

    private void OnEnable()
    {
        _socket.OnGetMessange += GetMessange;
    }

    private void OnDisable()
    {
        _socket.OnGetMessange -= GetMessange;
    }

    public void AddAction(string key, System.Action<string> action)
    {
        _actions.Add(key, action);
    }

    public void StartRoom(ServerJoinRoom join, System.Action<string> action)
    {
        var messange = MessageData.JsonMessange("srv_ready", 
            JsonConvert.SerializeObject(join));
        _socket.SendRequest(messange);
    }
    public void GetRoomPlayer(UserData data, System.Action<string> action)
    {
        var messange = MessageData.JsonMessange("get_RoomPlayers", 
            JsonConvert.SerializeObject(data));
        _socket.SendRequest("roomPlayersID", messange, action);
    }

    public void ExitRoom(ServerExitRoom data)
    {
        var json = MessageData.JsonMessange("srv_exit", JsonConvert.SerializeObject(data));
        _socket.SendRequest(json);
    }

    private void GetMessange(MessageData messange)
    {
        foreach (var item in _actions)
        {
            if (item.Key == messange.eventType)
                item.Value.Invoke(messange.data);
        }
    }

}
