using UnityEngine;
using TMPro;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField] private UserPreview _preview;
    [SerializeField] private TextMeshProUGUI _id;

    public Player Content { get; private set; }

    public void BindPlayer(Player player)
    {
        Content = player;
        _preview.SetPlayer(player);
        if (player)
        {
            _id.SetText(player.Data.ID.ToString());
            _id.gameObject.SetActive(true);
        }
        else
        {
            _id.gameObject.SetActive(false);
        }
    }
}
