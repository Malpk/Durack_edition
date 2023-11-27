using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Room : MonoBehaviour
{
    [SerializeField] private RoomSkin _skin;
    [SerializeField] private TextMeshProUGUI _roomIdText;

    private Player _player;

    public RoomSkin Skin => _skin;

    public void InitializateRoom(JoinRoom room)
    {
        _roomIdText.SetText(room.RoomID.ToString());
    }

    public void Enter(Player player)
    {
        gameObject.SetActive(true);
    }

    public void Exit(Player player)
    {
        gameObject.SetActive(false);
    }

}
