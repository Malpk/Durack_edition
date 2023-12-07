using UnityEngine;
using Newtonsoft.Json;

public class Table : MonoBehaviour
{
    [SerializeField] private TableUI _table;
    [SerializeField] private TableDropArea _tableTriger;
    [SerializeField] private RoomRow _enemys;
    [SerializeField] private Coloda _coloda;
    [SerializeField] private PlayerPanel _player;
    [SerializeField] private TableSocket _socket;

    private uint _roomId;
    private GameCard _card;

    private void Awake()
    {
        _socket.AddAction("cl_role", UpdateRole);
        _socket.AddAction("cl_ThrowedCard", ThrowCard);
    }

    private void OnEnable()
    {
        _tableTriger.OnDropCard += ThrowRequest;
    }

    private void OnDisable()
    {
        _tableTriger.OnDropCard -= ThrowRequest;
    }

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


    private void ThrowRequest(GameCard card)
    {
        _card = card;
        _socket.SendThrowCard(new RequestThrow
        {
            card = card.Data,
            RoomID = _roomId,
            UserID = _player.Content.Data.ID
        });
    }

    private void ThrowCard(string json)
    {
        var data = JsonConvert.DeserializeObject<ClientCardData>(json);
        var card = GetCard(data.card);
        _table.PlaceCard(card);
    }

    private GameCard GetCard(Card card)
    {
        if (_card.Data.suit == card.suit && _card.Data.nominal == card.nominal)
            return _card;
        else
            return _coloda.CreateCard(card);
    }

    public void UpdateRole(string json)
    {
        var roles = JsonConvert.DeserializeObject<RoleData[]>(json);
        foreach (var role in roles)
        {
            if (_player.ID == role.UserID)
            {
                _player.SetRole(role.Role);
                _tableTriger.SetMode(role.Role == ERole.FirstThrower);
            }
            else
            {
                _enemys.SetRole(role);
            }
        }
    }

    public void TakeCard(GameCard card)
    {
        _player.AddCard(card);
    }

    public void TakeCard(GameCard card, uint id)
    {
        _enemys.GetPlayer(id).AddCard(card);
    }

}
