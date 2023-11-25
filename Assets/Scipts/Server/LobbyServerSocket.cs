using Client;
using UnityEngine;
using Newtonsoft.Json;

public class LobbyServerSocket : MonoBehaviour
{
    [SerializeField] private string _key;
    [Header("Reference")]
    [SerializeField] private Lobby _lobby;
    [SerializeField] private SocketServer _socket;

    private void Awake()
    {
        _lobby.Rooms.OnEnterRoom += OnEnterRoom;
    }

    private void OnDestroy()
    {
        _lobby.Rooms.OnEnterRoom -= OnEnterRoom;
    }


    private void Start()
    {
        if (PlayerPrefs.HasKey(_key))
        {
            var userJson = PlayerPrefs.GetString(_key);
            var data = JsonConvert.DeserializeObject<UserData>(userJson);
            _lobby.Initializate(data);
            PlayerPrefs.DeleteKey(_key);
            var json = JsonConvert.SerializeObject(new Server.UserData()
            {
                token = data.Token,
                UserID = data.ID
            });
            _socket.SendRequest("Chips", CreateMessange("getChips", json), SetChip);
            _socket.SendRequest("cl_getImage", CreateMessange("getAvatar", json), SetAvatar);
            _socket.SendRequest("FreeRooms" , CreateMessange("getFreeRooms", json), UpdateRooms);
        }
    }


    private void OnEnterRoom(uint key, System.Action<string> arg2)
    {
        Debug.Log("Enter");
    }


    public string CreateMessange(string key, string json)
    {
        var messange = new MessageData(key, json);
        return JsonConvert.SerializeObject(messange);
    }

    #region avatar
    private void SetAvatar(string json)
    {
        var data = JsonConvert.DeserializeObject<AvatarData>(json);
        _lobby.SetAvatar(data.UserID, GetImage(data.AvatarImage));
    }

    private Sprite GetImage(string json)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(System.Convert.FromBase64String(json));
        return Sprite.Create(texture,  new Rect(0, 0, texture.width,
            texture.height), Vector2.one * 0.5f);
    }
    #endregion

    private void SetChip(string json)
    {
        var data = JsonConvert.DeserializeObject<ClientData>(json);
        _lobby.Player.SetChip(data);
    }

    private void UpdateRooms(string json)
    {
        var freeRoomsID = JsonConvert.DeserializeObject<FreeRooms>(json);
        if (freeRoomsID.FreeRoomsID != null)
            _lobby.Rooms.UpdateRoom(freeRoomsID.FreeRoomsID);
        else
            Debug.LogError("is not freeRooms");
    }


}
