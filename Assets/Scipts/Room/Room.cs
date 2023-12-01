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
    [SerializeField] private Table _table;
    [SerializeField] private RoomHUD _hud;
    [SerializeField] private RoomSkin _skin;
    [SerializeField] private RoomSocket _soket;
    [SerializeField] private RoomStartPanel _startPanel;
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
        _startPanel.OnStart += StartGame;
    }

    private void OnDestroy()
    {
        _exitButton.onClick.RemoveAllListeners();
        _startPanel.OnStart -= StartGame;
    }

    public void InitializateRoom(JoinRoom room)
    {
        _roomData = room;
        _roomIdText.SetText("ID:" + room.RoomID.ToString());
    }
    #region Mode

    public void StartGame()
    {
        var readyData = new ServerJoinRoom()
        {
            RoomID = _roomData.RoomID
        };
        _soket.StartRoom(readyData, OnStartRoom);
    }

    private void OnStartRoom(string json)
    {
        var data = JsonConvert.DeserializeObject<ClientReady>(json);
        _startPanel.SetMode(false);
    }

    private void SetStartMode(string json)
    {
        var players = JsonConvert.DeserializeObject<PlayersInRoom>(json).PlayersID;
        _startPanel.SetStartMode(players.Length == _roomData.maxPlayers);
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
        _table.AddPlayer(player);
        _startPanel.SetStartMode(false);
        gameObject.SetActive(true);
        _soket.GetRoomPlayer(new Server.UserData()
        {
            UserID = _player.Data.ID,
            RoomID = _roomData.RoomID
        }, SetStartMode);
    }

    public void Exit()
    {
        _table.RemovePlayer();
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
        _table.AddEnemy(player);
        _enemys.Add(player);
        Debug.LogWarning(_roomData.maxPlayers);
        _startPanel.SetStartMode(_enemys.Count + 1 == _roomData.maxPlayers);
    }

    public void RemoveEnemy(string json)
    {
        var enemy = GetEnemys(JsonConvert.
            DeserializeObject<PlayerExit>(json).uid);
        _table.RemoveEnemy(enemy);
        _enemys.Remove(enemy);
        Destroy(enemy);
        _startPanel.SetStartMode(false);
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
