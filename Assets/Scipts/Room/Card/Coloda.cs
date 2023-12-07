using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Coloda : MonoBehaviour
{
    [SerializeField] private string _stylePath;
    [SerializeField] private GameCard _card;
    [SerializeField] private CardItem _back;
    [SerializeField] private SuitItem[] _suits;
    [Header("Reference")]
    [SerializeField] private Table _table;
    [SerializeField] private Image _trump;
    [SerializeField] private Canvas _tableCanvas;
    [SerializeField] private Transform _cardHolder;
    [SerializeField] private TableSocket _socket;

    private List<GameCard> _poolCard = new List<GameCard>();

    private void Awake()
    {
        _socket.AddAction("clienReady", SetTrump);
        _socket.AddAction("GetCard", CreateCard);
        _socket.AddAction("cl_gotCard", CreateBack);
    }

    public void SetTrump(string json)
    {
        var ready = JsonConvert.DeserializeObject<ClientReady>(json);
        _trump.sprite = GetSprite(ready.trump);
    }

    public GameCard CreateCard(Card data)
    {
        var card = GetCard();
        card.transform.position = transform.position;
        card.BindCard(GetSprite(data), data);
        card.OnDelete += ReturnCard;
        return card;
    }

    private void CreateBack(string json)
    {
        var data = JsonConvert.DeserializeObject<UserClient>(json);
        var card = GetCard();
        card.transform.position = transform.position;
        card.BindCard(_back.GetSprite(_stylePath));
        _table.TakeCard(card, data.UserID);
    }

    private void CreateCard(string json)
    {
        var data = JsonConvert.DeserializeObject<Card>(json);
        var card = CreateCard(data);
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
        var newCard = Instantiate(_card.gameObject, _cardHolder).GetComponent<GameCard>();
        newCard.Initializate(_tableCanvas);
        return newCard;
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
