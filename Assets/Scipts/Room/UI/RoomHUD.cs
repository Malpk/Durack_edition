using UnityEngine;
using UnityEngine.UI;

public class RoomHUD : MonoBehaviour
{
    [SerializeField] private Button _start;
    [SerializeField] private RoomRow _row;
    [SerializeField] private PlayerPanel _panel;

    public void SetStartMode(bool mode)
    {
        _start.interactable = mode;
    }

    public void SetPlayer(Player player)
    {
        _panel.BindPlayer(player);
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
