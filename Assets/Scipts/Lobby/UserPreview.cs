using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserPreview : MonoBehaviour
{
    [SerializeField] private Image _temp;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _name;

    public void SetPlayer(Player avatar)
    {
        SetName(avatar);
        SetAvatar(avatar ? avatar.Sprite : null);
    }

    public void SetAvatar(Sprite sprite)
    {
        _icon.sprite = sprite;
        _icon.gameObject.SetActive(sprite);
        _temp.gameObject.SetActive(!sprite);
    }

    public void SetName(Player player)
    {
        if(player)
            _name.SetText(player.Data.Login);
        _name.gameObject.SetActive(player);
    }
}

