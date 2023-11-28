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
        _exitButton.onClick.AddListener(() => Exit(_player));
    }

    private void OnDestroy()
    {
        _exitButton.onClick.RemoveAllListeners();
    }

    public void InitializateRoom(JoinRoom room)
    {
        _hud.ShowEnemys(room.maxPlayers - 1);
        _roomIdText.SetText("ID:" + room.RoomID.ToString());
    }

    public void Enter(Player player)
    {
        _player = player;
        _hud.SetPlayer(player);
        gameObject.SetActive(true);
    }

    public void Exit(Player player)
    {
        _player = null;
        gameObject.SetActive(false);
        _lobby.gameObject.SetActive(true);
        _soket.ExitRoom(new ServerExitRoom()
        {
            rid = _roomData.RoomID,
            token = player.Data.Token
        });
    }

    public void AddEnemy(string json)
    {
        var player = Instantiate(_prefab.gameObject, transform).
            GetComponent<Player>();
        player.BindPlayer(new UserData()
        {
            ID = JsonConvert.DeserializeObject<PlayerJoin>(json).playerID
        });
        _hud.AddEnemy(player);
        _enemys.Add(player);
    }

    public void RemoveEnemy(string json)
    {
        var player = GetEnemys(JsonConvert.
            DeserializeObject<PlayerExit>(json).uid);
        _hud.RemoveEnemy(player);
        _enemys.Remove(player);
        Destroy(player);
    }

    private Player GetEnemys(uint id)
    {
        Debug.LogWarning(id);
        foreach (var enemy in _enemys)
        {
            if (enemy.Data.ID == id)
                return enemy;
        }
        return null;
    }
}
