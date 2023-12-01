using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CardHolder : MonoBehaviour
{
    [SerializeField] private int _maxCount = 6;
    [SerializeField] private GameObject _pointPrefab;
    [Header("Reference")]
    [SerializeField] private GridLayoutGroup _grid;

    private Dictionary<GameCard, Transform> _cards = new Dictionary<GameCard, Transform>();
    private List<Transform> _pools = new List<Transform>();

    public void Add(GameCard card)
    {
        var point = GetPoint();
        card.MoveTo(point);
        card.OnChangeHolder += Remove;
        _cards.Add(card, point);
    }

    public void Remove(GameCard card)
    {
        card.OnChangeHolder -= Remove;
        _pools.Add(GetPoint(card));
        _cards.Remove(card);
    }

    private Transform GetPoint(GameCard card)
    {
        foreach (var item in _cards)
        {
            if (item.Key == card)
                return item.Value;
        }
        return null;
    }

    private Transform GetPoint()
    {
        if (_pools.Count > 0)
        {
            var point = _pools[0];
            _pools.RemoveAt(0);
            return point;
        }
        return Instantiate(_pointPrefab, _grid.transform).transform;
    }

}
