using UnityEngine;

public class RoomRow : MonoBehaviour
{
    [SerializeField] private PlayerPanel[] _playerPanel;

    public void Clear()
    {
        foreach (var panel in _playerPanel)
        {
            if(panel.Content)
            {
                panel.gameObject.SetActive(false);
                panel.BindPlayer(null);
            }
        }
    }

    public void AddPlayer(Player player)
    {
        var panel = GetFreePanel();
        panel.BindPlayer(player);
        panel.gameObject.SetActive(true);
    }

    public void RemovePlayer(Player player)
    {
        var panel = GetPlayer(player.Data.ID);
        panel.BindPlayer(null);
        panel.gameObject.SetActive(false);
    }


    public PlayerPanel GetPlayer(uint id)
    {
        foreach (var panel in _playerPanel)
        {
            if (panel.ID == id)
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
