using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Button _backButton;
    [SerializeField] private InterfaceSwitcher _switcher;

    private void Awake()
    {
        //var data = new RoomData()
        //{
        //    RoomId = 45454,
        //    RoomSize = 6,
        //    CountPlayer = 2,
        //    Bet = 500,
        //    Type = 0
        //};

        //var rooms = new string[]
        //{
        //    JsonConvert.SerializeObject(data),
        //    JsonConvert.SerializeObject(data),
        //    JsonConvert.SerializeObject(data)
        //};

        //Debug.Log(JsonConvert.SerializeObject(MessageData.
        //    JsonMessange("FreeRooms", JsonConvert.SerializeObject(rooms))));
        ShowStartMenu();
        _backButton.onClick.AddListener(ShowStartMenu);
    }

    private void OnDestroy()
    {
        _backButton.onClick.RemoveAllListeners();
    }

    public void ShowRegistrationMenu()
    {
        Show(MenuType.Registration);
    }

    public void ShowAutoMenu()
    {
        Show(MenuType.Auto);
    }

    private void Show(MenuType menu)
    {
        _switcher.SwitchMenu(menu);
        _backButton.gameObject.SetActive(true);
    }

    private void ShowStartMenu()
    {
        _backButton.gameObject.SetActive(false);
        _switcher.SwitchMenu(MenuType.None);
    }
}
