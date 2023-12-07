using UnityEngine;
using TMPro;

public class RoomHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerBet;
    [SerializeField] private TextMeshProUGUI _roomBet;

    public void Initilizate(RoomData room)
    {
        _playerBet.SetText(room.Bet.ToString());
        _roomBet.SetText((room.Bet * room.RoomSize).ToString());
        Debug.LogWarning(room.Bet);
    }
}
