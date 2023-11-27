using UnityEngine;
using UnityEngine.UI;

public class RoomSkin : MonoBehaviour
{
    [SerializeField] private Image _table;
    [SerializeField] private Image _background;

    public void SetSkin(Sprite table, Sprite background)
    {
        _table.sprite = table;
        _background.sprite = background;
    }
}
