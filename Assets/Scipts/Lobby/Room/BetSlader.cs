using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class BetSlader : MonoBehaviour
{
    [SerializeField] private BetItem[] _bets;
    [Header("Reference")]
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _minBet;
    [SerializeField] private TextMeshProUGUI _maxBet;
    [SerializeField] private TextMeshProUGUI _curretBet;

    public event System.Action<uint> OnBet;

    public uint Bet => GetBet().Bet;

    private void OnValidate()
    {
        if (_bets.Length > 0)
        {
            _minBet?.SetText(_bets[0].Name);
            _maxBet?.SetText(_bets[_bets.Length - 1].Name);
        }
        else
        {
            _minBet?.SetText("");
            _maxBet?.SetText("");
        }
    }

    private void Awake()
    {
        _slider.onValueChanged.AddListener(OnSetBet);
    }

    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveAllListeners();
    }

    public void Load(float value)
    {
        _slider.value = value;
    }

    public float Save()
    {
        return _slider.value;
    }

    private void OnSetBet(float value)
    {
        var bet = GetBet();
        _curretBet.SetText(bet.Name);
        OnBet?.Invoke(bet.Bet);
    }

    private BetItem GetBet()
    {
        var index = Mathf.RoundToInt(_slider.value * (_bets.Length - 1));
        return _bets[index];
    }
}
