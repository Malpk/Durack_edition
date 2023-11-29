using Newtonsoft.Json;
using UnityEngine;
using Client;
using System.Collections.Generic;

public class Lobby : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private LobbyUI _menu;
    [SerializeField] private Room _room;
    [SerializeField] private RoomList _rooms;
    [SerializeField] private RoomCreatePanel _createPanel;
    [Header("Server")]
    [SerializeField] private LobbyServerSocket _socket;

    private bool _isStart;

    public string Token { get; private set; }


    private void Awake()
    {
        var data = _socket.Load();
        Token = data.Token;
        _player.BindPlayer(data);
    }

    private void OnEnable()
    {
        if(_isStart)
            _socket.RequestFreeRoom(_player.Data, UpdateRooms);
        _player.OnUpdateChips += _menu.SetChip;
        _rooms.OnEnterRoom += OnEnterRoom;
        _createPanel.OnCreateRoom += OnCreateRoom;
    }

    private void OnDisable()
    {
        _player.OnUpdateChips -= _menu.SetChip;
        _rooms.OnEnterRoom -= OnEnterRoom;
        _createPanel.OnCreateRoom -= OnCreateRoom;
    }

    public void Start()
    {
        _isStart = true;
        _menu.SetName(_player);
        _socket.RequestAvatar(_player.Data, SetAvatar);
        _socket.RequestChips(_player.Data, SetChip);
        _socket.RequestFreeRoom(_player.Data, UpdateRooms);
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
        var jsons = JsonConvert.DeserializeObject<string[]>(json);
        var list = new List<RoomData>();
        foreach (var data in jsons)
        {
            list.Add(JsonConvert.DeserializeObject<RoomData>(data));
        }
        _rooms.UpdateRoom(list.ToArray());
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
