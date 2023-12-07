using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private float _smoothMove;
    [Header("Reference")]
    [SerializeField] private Image _icon;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private RectTransform _rect;
    [SerializeField] private RectTransform _beatPoint;

    private bool _isDrag;

    private Coroutine _corotine;
    private RectTransform _rectPoint;

    public event System.Action<GameCard> OnDelete;
    public event System.Action<GameCard> OnChangeHolder;

    public Player Owner { get; private set; }
    public Card Data { get; private set; }

    #region Drag
    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDrag = true;
        if (_corotine != null)
            StopCoroutine(_corotine);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rect.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDrag = false;
        if (_rectPoint != null)
        {
            _corotine = StartCoroutine(Move(_rectPoint));
        }
    }

    #endregion
    public void BindCard(Sprite sprite, Card card = null)
    {
        _icon.sprite = sprite;
        Data = card;
    }

    public void Initializate(Canvas canvas)
    {
        _canvas = canvas;
    }

    public void MoveTo(RectTransform point)
    {
        if (_corotine != null)
            StopCoroutine(_corotine);
        OnChangeHolder?.Invoke(this);
        _rectPoint = point;
        if (!_isDrag)
            _corotine = StartCoroutine(Move(point));
    }

    private IEnumerator Move(RectTransform point)
    {
        var velocity = Vector3.zero;
        var velocitySize = Vector2.zero;
        while (true)
        {
            _rect.position = Vector3.SmoothDamp(_rect.position, 
                point.position, ref velocity, _smoothMove);
            _rect.sizeDelta = Vector2.SmoothDamp(_rect.sizeDelta, point.sizeDelta, 
                ref velocitySize, _smoothMove);
            yield return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (gameObject == eventData.pointerDrag)
        {
            return;
        }
        else if (eventData.pointerDrag.TryGetComponent(out GameCard card))
        {
            card.MoveTo(_beatPoint);
        }
    }
}
