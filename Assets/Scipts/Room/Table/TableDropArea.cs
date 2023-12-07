using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TableDropArea : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image _table;

    public event System.Action<GameCard> OnDropCard;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out GameCard card))
        {
            OnDropCard?.Invoke(card);
        }
    }

    public void SetMode(bool mode)
    {
        _table.enabled = mode;
    }

}
