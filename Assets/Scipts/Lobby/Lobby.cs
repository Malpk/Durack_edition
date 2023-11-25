using UnityEngine;
using Client;

public class Lobby : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private LobbyUI _menu;
    [SerializeField] private RoomList _rooms;
    public string Token { get; private set; }
    public Player Player => _player;
    public RoomList Rooms => _rooms;

    private void Awake()
    {
        _player.OnUpdateChips += _menu.SetChip;
    }

    private void OnDestroy()
    {
        _player.OnUpdateChips -= _menu.SetChip;
    }

    public void Initializate(UserData data)
    {
        Token = data.Token;
        _player.BindPlayer(data);
        _menu.SetName(data.Login);
    }

    public void SetAvatar(uint id, Sprite incon)
    {
        if (id == _player.Data.ID)
        {
            _menu.SetAvatar(incon);
        }
        else
        {
            Debug.LogError("is wrong id");
        }
    }

    public void UpdateRooms(uint[] rooms)
    { 
    }
}
