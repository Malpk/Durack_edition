using UnityEngine;

public class RoomRow : MonoBehaviour
{
    [SerializeField] private PlayerPanel[] _playerPanel;


    public void AddPlayer(Player player)
    {
        var panel = GetFreePanel();
        panel.BindPlayer(player);
        panel.gameObject.SetActive(true);
    }

    public void RemovePlayer(Player player)
    {
        var panel = GetPanel(player.Data.ID);
        panel.BindPlayer(null);
        panel.gameObject.SetActive(false);
    }

    private PlayerPanel GetPanel(uint id)
    {
        foreach (var panel in _playerPanel)
        {
            if (panel.Content.Data.ID == id)
                return panel;
        }
        return null;
    }

    private PlayerPanel GetFreePanel()
    {
        foreach (var panel in _playerPanel)
        {
            if (!panel.Content)
                return panel;
        }
        return null;
    }
}
