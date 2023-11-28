using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomHUD : MonoBehaviour
{
    [SerializeField] private Button _start;
    [SerializeField] private RoomRow _row;
    [SerializeField] private PlayerPanel _panel;
    [SerializeField] private TextMeshProUGUI _playerBet;
    [SerializeField] private TextMeshProUGUI _roomBet;

    public void Initilizate(Player player ,JoinRoom room)
    {
        _panel.BindPlayer(player);
        _playerBet.SetText(room.bet.ToString());
        _roomBet.SetText((room.bet * room.maxPlayers).ToString());
    }

    public void SetStartMode(bool mode)
    {
        _start.interactable = mode;
    }

    public void AddEnemy(Player player)
    {
        _row.AddPlayer(player);
    }

    public void RemoveEnemy(Player player)
    {
        _row.RemovePlayer(player);
    }
}
