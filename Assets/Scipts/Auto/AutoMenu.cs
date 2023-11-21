using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class AutoMenu : MonoBehaviour
{
    [SerializeField] private string _saveDataKey = "key";
    [SerializeField] private string _requestKey;
    [Header("Reference")]
    [SerializeField] private UIMenu _menu;
    [SerializeField] private Toggle _toogleRemeabeMe;
    [SerializeField] private Button _autoButton;
    [SerializeField] private TMP_InputField _login;
    [SerializeField] private TMP_InputField _password;

    public UIMenu Menu => _menu;
    public event System.Action<string,string> OnAuto;

    private void Reset()
    {
        _saveDataKey = "key";
        _requestKey = "Emit_login";
    }

    private void Awake()
    {
        _autoButton.interactable = false;
        _autoButton.onClick.AddListener(Auto);
        _login.onValueChanged.AddListener(UpdateAutoButton);
        _password.onValueChanged.AddListener(UpdateAutoButton);
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(_saveDataKey))
        {
            var data = JsonUtility.FromJson<ClientLogin>(
                PlayerPrefs.GetString(_saveDataKey));
            _login.text = data.name;
            _password.text = data.password;
        }
    }

    private void OnDestroy()
    {
        _autoButton.onClick.RemoveAllListeners();
    }

    private void Auto()
    {
        var data = new ClientLogin(_login.text, _password.text);
        if (_toogleRemeabeMe.isOn)
            PlayerPrefs.SetString(_saveDataKey, JsonUtility.ToJson(data));
        else
            PlayerPrefs.DeleteKey(_saveDataKey);
        OnAuto?.Invoke(_requestKey, JsonUtility.ToJson(data));
    }

    private void UpdateAutoButton(string text)
    {
        _autoButton.interactable = DataFillter.CheakLogin(_login.text);
        _autoButton.interactable &= DataFillter.CheakPassword(_password.text);
    }
}
