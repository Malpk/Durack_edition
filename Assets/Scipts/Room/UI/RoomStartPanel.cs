using UnityEngine;
using UnityEngine.UI;

public class RoomStartPanel : MonoBehaviour
{
    [SerializeField] private Button _startButton;

    public event System.Action OnStart;

    private void Awake()
    {
        _startButton.onClick.AddListener(() => OnStart?.Invoke());
    }

    private void OnDestroy()
    {
        _startButton.onClick.RemoveAllListeners();
    }

    public void SetMode(bool mode)
    {
        gameObject.SetActive(mode);
    }

    public void SetStartMode(bool mode)
    {
        _startButton.interactable = mode;
    }
}
