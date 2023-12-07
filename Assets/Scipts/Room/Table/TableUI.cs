using UnityEngine;
using System.Collections.Generic;

public class TableUI : MonoBehaviour
{
    [SerializeField] private CardPoint _pointPrefab;

    private List<CardPoint> _point = new List<CardPoint>();
    private List<CardPoint> _pool = new List<CardPoint>();

    public event System.Action<GameCard> OnPlace;

    public void PlaceCard(GameCard card)
    {
        var point = GetPoint();
        point.BindCard(card);
        _point.Add(point);
    }

    public GameCard GrabCard()
    {
        if (_point.Count > 0)
        {
            var card = _point[0].Content;
            _point[0].Clear();
            _point[0].gameObject.SetActive(false);
            _point.RemoveAt(0);
            return card;
        }
        return null;
    }

    private CardPoint GetPoint()
    {
        if(_pool.Count > 0)
        {
            var point = _pool[0];
            point.gameObject.SetActive(true);
            return point;
        }
        return Instantiate(_pointPrefab.gameObject, transform).GetComponent<CardPoint>();
    }
}
