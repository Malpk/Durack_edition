using UnityEngine;
using TMPro;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private UIMenu _menu;
    [SerializeField] private UserPreview _preview;
    [SerializeField] private TextMeshProUGUI _chips;

    public void SetName(string login)
    {
        _preview.SetName(login);
    }

    public void SetAvatar(Sprite avatar)
    {
        _preview.SetAvatar(avatar);
    }

    public void SetChip(int chips)
    {
        _chips.SetText(chips.ToString());
    }
}
