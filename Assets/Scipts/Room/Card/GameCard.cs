using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameCard : MonoBehaviour
{
    [SerializeField] private float _smoothMove;
    [Header("Reference")]
    [SerializeField] private Image _icon;

    private Coroutine _corotine;

    public event System.Action<GameCard> OnDelete;
    public event System.Action<GameCard> OnChangeHolder;

    public void BindCard(Sprite sprite)
    {
        _icon.sprite = sprite;
    }

    public void MoveTo(Transform point)
    {
        if (_corotine != null)
        {
            StopCoroutine(_corotine);    
        }
        OnChangeHolder?.Invoke(this);
        _corotine = StartCoroutine(Move(point));
    }

    private IEnumerator Move(Transform point)
    {
        var velocity = Vector3.zero;
        Debug.Log(point);
        while (true)
        {
            transform.position = Vector3.SmoothDamp(transform.position, 
                point.position, ref velocity, _smoothMove);
            yield return null;
        }
    }
}
