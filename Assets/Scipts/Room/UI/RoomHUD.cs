using UnityEngine;

public class RoomHUD : MonoBehaviour
{
    [SerializeField] private PlayerPanel _panel;
    [SerializeField] private RoomRow _row;

    public void ShowEnemys(int count)
    {
        _row.Show(count);
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
