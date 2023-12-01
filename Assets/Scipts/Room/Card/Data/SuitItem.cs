using UnityEngine;

[System.Serializable]
public class SuitItem
{
    [SerializeField] private ESuit _suit;
    [SerializeField] private string _path;
    [SerializeField] private CardItem[] _cards;

    public ESuit Suit => _suit;

    public Sprite GetSprite(string stylePath,ENominal nominal)
    {
        foreach (var card in _cards)
        {
            if (card.Nominal == nominal)
                return card.GetSprite(stylePath + "/" + _path);
        }
        return null;
    }
}
