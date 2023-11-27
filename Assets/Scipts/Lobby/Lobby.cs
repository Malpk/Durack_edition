using Newtonsoft.Json;
using UnityEngine;
using Client;


public class Lobby : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private LobbyUI _menu;
    [SerializeField] private Room _room;
    [SerializeField] private RoomList _rooms;
    [SerializeField] private RoomCreatePanel _createPanel;
    [Header("Server")]
    [SerializeField] private LobbyServerSocket _socket;

    public string Token { get; private set; }

    private void OnEnable()
    {
        _player.OnUpdateChips += _menu.SetChip;
        _rooms.OnEnterRoom += OnEnterRoom;
        _createPanel.OnCreateRoom += OnCreateRoom;
    }



    private void OnDisable()
    {
        _player.OnUpdateChips -= _menu.SetChip;
        _rooms.OnEnterRoom -= OnEnterRoom;
    }

    public void Start()
    {
        var data = _socket.Load();
        Token = data.Token;
        _player.BindPlayer(data);
        _menu.SetName(data.Login);
        _socket.RequestFreeRoom(data, UpdateRooms);
        _socket.RequestAvatar(data, SetAvatar);
        _socket.RequestChips(data, SetChip);
        _socket.AddAction("FreeRooms", UpdateRooms);
    }

    #region Data
    private void SetChip(string json)
    {
        var chip = JsonConvert.DeserializeObject<ClientData>(json);
        _player.SetChip(chip);
    }

    private void UpdateRooms(string json)
    {
        var freeRooms = JsonConvert.DeserializeObject<FreeRooms>(json);
        _rooms.UpdateRoom(freeRooms.FreeRoomsID);
    }

    private void SetAvatar(string json)
    {
        var avatar = JsonConvert.DeserializeObject<AvatarData>(json);
        if (avatar.UserID == _player.Data.ID)
        {
            _menu.SetAvatar(GetImage(avatar.AvatarImage));
        }
        else
        {
            Debug.LogError("is wrong id");
        }
    }

    private Sprite GetImage(string json)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(System.Convert.FromBase64String(json));
        return Sprite.Create(texture, new Rect(0, 0, texture.width,
            texture.height), Vector2.one * 0.5f);
    }
    #endregion

    #region Action

    public void OnEnterRoom(uint roomId)
    {
        _socket.EnterRoom(roomId, _player.Data, JoinRoom);
    }


    private void JoinRoom(string json)
    {
        var data = JsonConvert.DeserializeObject<JoinRoom>(json);
        _room.InitializateRoom(data);
        _room.Enter(_player);
        gameObject.SetActive(false);
    }

    private void OnCreateRoom(ServerCreateRoom data)
    {
        data.roomOwner = _player.Data.ID;
        data.token = _player.Data.Token;
        _socket.CreateRoom(data, JoinRoom);
    }
    #endregion
}
