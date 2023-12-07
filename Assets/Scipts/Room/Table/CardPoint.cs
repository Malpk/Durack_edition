using UnityEngine;

public class CardPoint : MonoBehaviour
{
    [SerializeField] private RectTransform _rectPosition;

    public GameCard Content { get; private set; }

    public void BindCard(GameCard card)
    {
        Content = card;
        card.MoveTo(_rectPosition);
    }

    public void Clear()
    {
        Content = null;
    }
}
