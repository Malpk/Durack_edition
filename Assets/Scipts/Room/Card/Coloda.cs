using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Coloda : MonoBehaviour
{
    [SerializeField] private string _stylePath;
    [SerializeField] private GameCard _card;
    [SerializeField] private CardItem _back;
    [SerializeField] private SuitItem[] _suits;
    [Header("Reference")]
    [SerializeField] private Table _table;
    [SerializeField] private Image _trump;
    [SerializeField] private TableSocket _socket;

    private List<GameCard> _poolCard = new List<GameCard>();

    private void OnEnable()
    {
        _socket.OnReady += SetTrump;
        _socket.OnEnemyGetCard += CreateBack;
        _socket.OnPlayerGetCard += CreateCard;
    }

    private void OnDisable()
    {
        _socket.OnReady -= SetTrump;
        _socket.OnEnemyGetCard -= CreateBack;
        _socket.OnPlayerGetCard -= CreateCard;
    }

    public void SetTrump(ClientReady ready)
    {
        _trump.sprite = GetSprite(ready.trump);
    }

    private void CreateBack(UserClient user)
    {
        var card = GetCard();
        card.transform.position = transform.position;
        card.BindCard(_back.GetSprite(_stylePath));
        _table.TakeCard(card, user.UserID);
    }

    private void CreateCard(Card data)
    {
        var card = GetCard();
        card.transform.position = transform.position;
        card.BindCard(GetSprite(data));
        card.OnDelete += ReturnCard;
        _table.TakeCard(card);
    }

    private void ReturnCard(GameCard card)
    {
        card.OnDelete -= ReturnCard;
        card.gameObject.SetActive(false);
        card.transform.parent = transform;
    }

    private GameCard GetCard()
    {
        if (_poolCard.Count > 0)
        {
            var card = _poolCard[0];
            card.gameObject.SetActive(true);
            card.transform.parent = _table.transform; 
            _poolCard.RemoveAt(0);
            return card;
        }
        return Instantiate(_card.gameObject, _table.transform).GetComponent<GameCard>();
    }

    private Sprite GetSprite(Card card)
    {
        foreach (var suit in _suits)
        {
            if (suit.Suit == card.Suit)
                return suit.GetSprite(_stylePath, card.Nominal);
        }
        return null;
    }
    
}
