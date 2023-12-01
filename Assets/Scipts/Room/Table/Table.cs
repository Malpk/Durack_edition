using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] private RoomRow _enemys;
    [SerializeField] private PlayerPanel _player;
    [SerializeField] private TableSocket _socket;


    #region parts

    public void AddPlayer(Player player)
    {
        _player.BindPlayer(player);
    }

    public void RemovePlayer()
    {
        _player.BindPlayer(null);
        _enemys.Clear();
    }

    public void AddEnemy(Player player)
    {
        _enemys.AddPlayer(player);
    }
    public void RemoveEnemy(Player player)
    {
        _enemys.RemovePlayer(player);
    }

    #endregion

    public void TakeCard(GameCard card)
    {
        _player.AddCard(card);
    }

    public void TakeCard(GameCard card, uint id)
    {
        _enemys.GetPlayer(id).AddCard(card);
    }

}
