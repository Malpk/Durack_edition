using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Client;
using Newtonsoft.Json;

public class Room : MonoBehaviour
{
    [SerializeField] private Player _prefab;
    [Header("Reference")]
    [SerializeField] private Lobby _lobby;
    [SerializeField] private RoomHUD _hud;
    [SerializeField] private RoomSkin _skin;
    [SerializeField] private RoomSocket _soket;
    [Header("Reference")]
    [SerializeField] private Button _exitButton;
    [SerializeField] private TextMeshProUGUI _roomIdText;

    private Player _player;
    private List<Player> _enemys = new List<Player>();
    private JoinRoom _roomData;

    public RoomSkin Skin => _skin;

    private void Awake()
    {
        _soket.AddAction("cl_joinRoom", AddEnemy);
        _soket.AddAction("cl_leaveRoom", RemoveEnemy);
        _exitButton.onClick.AddListener(Exit);
    }

    private void OnDestroy()
    {
        _exitButton.onClick.RemoveAllListeners();
    }

    public void InitializateRoom(JoinRoom room)
    {
        _roomData = room;
        _roomIdText.SetText("ID:" + room.RoomID.ToString());
    }
    #region Mode

    private void SetStartMode(string json)
    {
        var players = JsonConvert.DeserializeObject<PlayersInRoom>(json).PlayersID;
        _hud.SetStartMode(players.Length == _roomData.maxPlayers);
        foreach (var id in players)
        {
            if(id != _player.Data.ID)
                AddEnemy(id);
        }
    }

    #endregion
    #region Player
    public void Enter(Player player)
    {
        _player = player;
        _hud.Initilizate(player, _roomData);
        _hud.SetStartMode(false);
        gameObject.SetActive(true);
        _soket.GetRoomPlayer(new Server.UserData()
        {
            UserID = _player.Data.ID,
            RoomID = _roomData.RoomID
        }, SetStartMode);
    }

    public void Exit()
    {
        gameObject.SetActive(false);
        _lobby.gameObject.SetActive(true);
        _soket.ExitRoom(new ServerExitRoom()
        {
            rid = _roomData.RoomID,
            token = _player.Data.Token
        });
        _player = null;
    }
    #endregion
    #region Enemys

    public void AddEnemy(string json)
    {
        var id = JsonConvert.DeserializeObject<PlayerJoin>(json).playerID;
        AddEnemy(id);
    }

    private void AddEnemy(uint id)
    {
        var player = Instantiate(_prefab.gameObject, transform).
             GetComponent<Player>();
        player.BindPlayer(new UserData()
        {
            ID = id
        });
        _hud.AddEnemy(player);
        _enemys.Add(player);
        _hud.SetStartMode(_enemys.Count + 1 == _roomData.maxPlayers);
        if (_enemys.Count + 1 > _roomData.maxPlayers)
            Debug.LogError("player is out range room size");
    }

    public void RemoveEnemy(string json)
    {
        var player = GetEnemys(JsonConvert.
            DeserializeObject<PlayerExit>(json).uid);
        _hud.RemoveEnemy(player);
        _enemys.Remove(player);
        Destroy(player);
        _hud.SetStartMode(false);
    }

    private Player GetEnemys(uint id)
    {
        foreach (var enemy in _enemys)
        {
            if (enemy.Data.ID == id)
                return enemy;
        }
        return null;
    }
    #endregion
}
