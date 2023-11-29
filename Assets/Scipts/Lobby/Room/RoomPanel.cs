using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomPanel : MonoBehaviour
{
    [SerializeField] private SpriteItem[] _sprites;
    [Header("Reference")]
    [SerializeField] private Image _icon;
    [SerializeField] private Button _joinButton;
    [SerializeField] private TextMeshProUGUI _id;
    [SerializeField] private TextMeshProUGUI _roomProgress;

    public uint ID => Data.RoomId;
    public RoomData Data { get; private set; }


    public event System.Action<uint> OnEnter;

    private void Awake()
    {
        _joinButton.onClick.AddListener(Enter);
    }

    private void OnDestroy()
    {
        _joinButton.onClick.RemoveAllListeners();
    }

    public void Bind(RoomData data)
    {
        Data = data;
        _icon.sprite = GetIcon(data.Type);
        _roomProgress.SetText($"{data.CountPlayer}/{data.RoomSize}");
        _id.SetText(data.RoomId.ToString());
    }

    private void Enter()
    {
        OnEnter?.Invoke(ID);
    }

    private Sprite GetIcon(int type)
    {
        foreach (var item in _sprites)
        {
            if (item.Type == type)
                return item.Icon;
        }
        return null;
    }
}
