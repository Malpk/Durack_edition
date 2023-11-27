using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class RoomCreatePanel : MonoBehaviour
{
    [SerializeField] private ServerCreateRoom _roomSetting;
    [SerializeField] private string _key = "roomSetting";
    [SerializeField] private uint[] _bets;
    [Header("UIReference")]
    [SerializeField] private Slider _betSlider;
    [SerializeField] private Button _createRoom;
    [SerializeField] private TMP_Dropdown _mode;
    [SerializeField] private TMP_Dropdown _typeRoom;
    [SerializeField] private TMP_Dropdown _maxPlayer;

    private int[] _maxCountsPlayers = new int[] { 2, 3, 4, 5, 6 };
    private uint[] _countCards = new uint[] { 24, 36, 52 };

    public event System.Action<ServerCreateRoom> OnCreateRoom;

    private void Awake()
    {
        _betSlider.onValueChanged.AddListener(SetBet);
        _mode.onValueChanged.AddListener(SetMode);
        _typeRoom.onValueChanged.AddListener(SetTypeRoom);
        _maxPlayer.onValueChanged.AddListener(SetCountPlayers);
        _createRoom.onClick.AddListener(CreateRoom);
        if (PlayerPrefs.HasKey(_key))
        {
            _roomSetting = JsonConvert.DeserializeObject<ServerCreateRoom>(PlayerPrefs.GetString(_key));
            _betSlider.value = _roomSetting.bet;
            _mode.value = _roomSetting.type;
            _typeRoom.value = _roomSetting.isPrivate;
            _maxPlayer.value = _roomSetting.maxPlayers;
        }
        else
        {
            SetBet(_betSlider.value);
            SetMode(_mode.value);
            SetTypeRoom(_typeRoom.value);
            SetCountPlayers(_maxPlayer.value);
        }
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetString(_key, JsonConvert.SerializeObject(_roomSetting));
        _betSlider.onValueChanged.AddListener(SetBet);
        _mode.onValueChanged.RemoveAllListeners();
        _typeRoom.onValueChanged.RemoveAllListeners();
        _maxPlayer.onValueChanged.RemoveAllListeners();
        _createRoom.onClick.RemoveAllListeners();
    }

    public void CreateRoom()
    {
        var setting = _roomSetting;
        setting.key = "";
        OnCreateRoom?.Invoke(setting);
    }

    private void SetBet(float value)
    {
       var index = Mathf.RoundToInt(value * (_bets.Length-1));
        _roomSetting.bet = _bets[index];
    }

    private void SetMode(int index)
    {
        _roomSetting.type = index;
    }

    private void SetCountPlayers(int index)
    {
        _roomSetting.maxPlayers = _maxCountsPlayers[index];
        _roomSetting.cards = GetCards(index);
    }

    private uint GetCards(int index)
    {
        index = Mathf.Clamp(index, 0, _countCards.Length - 1);
        return _countCards[index];
    }

    private void SetTypeRoom(int index)
    {
        _roomSetting.isPrivate = index;
    }
}
