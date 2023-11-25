using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomPanel : MonoBehaviour
{
    [SerializeField] private Button _joinButton;
    [SerializeField] private TextMeshProUGUI _id;

    public uint ID { get; private set; }

    public event System.Action<uint> OnEnter;

    private void Awake()
    {
        _joinButton.onClick.AddListener(Enter);
    }

    private void OnDestroy()
    {
        _joinButton.onClick.RemoveAllListeners();
    }

    public void Bind(uint id)
    {
        ID = id;
        _id.SetText(id.ToString());
    }

    private void Enter()
    {
        OnEnter?.Invoke(ID);
    }
}
