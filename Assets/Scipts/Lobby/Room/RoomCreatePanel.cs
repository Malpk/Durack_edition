using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class RoomCreatePanel : MonoBehaviour
{
    [SerializeField] private ServerCreateRoom _roomSetting;
    [SerializeField] private string _key = "roomSetting";

    [Header("UIReference")]
    [SerializeField] private BetSlader _betSlider;
    [SerializeField] private Button _createRoom;
    [SerializeField] private TMP_Dropdown _mode;
    [SerializeField] private TMP_Dropdown _typeRoom;
    [SerializeField] private TMP_Dropdown _maxPlayer;

    private int[] _maxCountsPlayers = new int[] { 2, 3, 4, 5, 6 };
    private uint[] _countCards = new uint[] { 24, 36, 52 };

    public event System.Action<ServerCreateRoom> OnCreateRoom;

    private void Awake()
    {
        if (PlayerPrefs.HasKey(_key))
        {
            var setting = JsonConvert.DeserializeObject<RoomCreateSetting>(PlayerPrefs.GetString(_key));
            _betSlider.Load(setting.Bet);
            _mode.value = setting.Type;
            _typeRoom.value = setting.IsPrivate;
            _maxPlayer.value = setting.MaxPlayers;
        }
        SetBet(_betSlider.Bet);
        SetMode(_mode.value);
        SetTypeRoom(_typeRoom.value);
        SetCountPlayers(_maxPlayer.value);
        SubcriteEvent();
    }

    private void OnDestroy()
    {
        var setting = new RoomCreateSetting()
        {
            IsPrivate = _typeRoom.value,
            Bet = _betSlider.Save(),
            Type = _mode.value,
            MaxPlayers = _maxPlayer.value
        };
        PlayerPrefs.SetString(_key, JsonConvert.SerializeObject(setting));
        UnsubcreteEvent();
    }

    private void SubcriteEvent()
    {
        _betSlider.OnBet += SetBet;
        _mode.onValueChanged.AddListener(SetMode);
        _typeRoom.onValueChanged.AddListener(SetTypeRoom);
        _maxPlayer.onValueChanged.AddListener(SetCountPlayers);
        _createRoom.onClick.AddListener(CreateRoom);
    }
    private void UnsubcreteEvent()
    {
        _betSlider.OnBet -= SetBet;
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

    private void SetBet(uint bet)
    {
        _roomSetting.bet = bet;
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
