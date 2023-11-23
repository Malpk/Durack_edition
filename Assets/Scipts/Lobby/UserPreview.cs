using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserPreview : MonoBehaviour
{
    [SerializeField] private Image _temp;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _name;

    public void SetAvatar(Sprite avatar)
    {
        _temp.gameObject.SetActive(!avatar);
        _icon.gameObject.SetActive(avatar);
        _icon.sprite = avatar;
    }

    public void SetName(string name)
    {
        _name.SetText(name);
    }
}
