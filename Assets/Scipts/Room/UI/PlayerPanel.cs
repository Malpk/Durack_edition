using UnityEngine;
using TMPro;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField] private ERole _role;
    [Header("Reference")]
    [SerializeField] private CardHolder _cards;
    [SerializeField] private UserPreview _preview;
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI _id;

    public uint ID => Content.Data.ID;
    public Player Content { get; private set; }

    public void AddCard(GameCard card)
    {
        _cards.Add(card);
    }

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

    public void SetRole(ERole role)
    {
        _role = role;
    }
}
