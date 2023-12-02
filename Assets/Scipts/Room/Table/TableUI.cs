using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class TableUI : MonoBehaviour, IDropHandler
{
    [SerializeField] private RectTransform _pointPrefab;

    private List<Transform> _pool = new List<Transform>();

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out GameCard card))
        {
            card.MoveTo(GetPoint());
        }
    }

    private RectTransform GetPoint()
    {
        var point = Instantiate(_pointPrefab.gameObject, transform);
        return point.GetComponent<RectTransform>();
    }
}
