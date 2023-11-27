using Client;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public class LobbyServerSocket : MonoBehaviour
{
    [SerializeField] private string _key;
    [Header("Reference")]
    [SerializeField] private SocketServer _socket;

    private Dictionary<string, System.Action<string>> _list = new Dictionary<string, System.Action<string>>();

    public event System.Action<uint[]> OnFreeRooms;

    private void OnEnable()
    {
        _socket.OnGetMessange += OnGetMessange;
    }

    private void OnDisable()
    {
        _socket.OnGetMessange -= OnGetMessange;
    }

    public UserData Load()
    {
        if (PlayerPrefs.HasKey(_key))
        {
            var userJson = PlayerPrefs.GetString(_key);
            var data = JsonConvert.DeserializeObject<UserData>(userJson);
            PlayerPrefs.DeleteKey(_key);
            return data;
        }
        return default;
    }

    public void AddAction(string key, System.Action<string> action)
    {
        _list.Add(key, action);
    }

    public void RequestChips(UserData data, System.Action<string> action)
    {
        var json = JsonConvert.SerializeObject(new Server.UserData(data));
        _socket.SendRequest("Chips", CreateMessange("getChips", json), action);
    }

    public void RequestAvatar(UserData data, System.Action<string> action)
    {
        var json = JsonConvert.SerializeObject(new Server.UserData(data));
        _socket.SendRequest("cl_getImage", CreateMessange("getAvatar", json), action);
    }
    public void RequestFreeRoom(UserData data, System.Action<string> action)
    {
        var json = JsonConvert.SerializeObject(new Server.UserData(data));
        _socket.SendRequest("FreeRooms", CreateMessange("getFreeRooms", json), action);
    }

    public void EnterRoom(uint roomId, UserData data, System.Action<string> action)
    {
        var joinRoomData = new ServerJoinRoom()
        {
            Token = data.Token,
            key = "",
            RoomID = roomId
        };
        var messange = CreateMessange("srv_joinRoom", JsonConvert.SerializeObject(joinRoomData));
        _socket.SendRequest("cl_enterInTheRoom", messange, action);
    }

    public void CreateRoom(ServerCreateRoom roomData, System.Action<string> action)
    {
        var messange = CreateMessange("srv_createRoom", JsonConvert.SerializeObject(roomData));
        _socket.SendRequest("cl_enterInTheRoomAsOwner", messange, action);
    }

    private string CreateMessange(string key, string json)
    {
        var messange = new MessageData(key, json);
        return JsonConvert.SerializeObject(messange);
    }

    private void OnGetMessange(MessageData messange)
    {
        foreach (var item in _list)
        {
            if (item.Key == messange.eventType)
                item.Value.Invoke(messange.data);
        }
    }
}
