using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class RegistrationMenu : MonoBehaviour
{
    [SerializeField] private string _requestKey;
    [SerializeField] private string _answerKey;
    [Header("Reference")]
    [SerializeField] private Button _registrationButton;
    [SerializeField] private TMP_InputField _eMail;
    [SerializeField] private TMP_InputField _login;
    [SerializeField] private TMP_InputField _password;
    [SerializeField] private InterfaceSwitcher _switcher;

    public event System.Action<string, string> OnRegistration;

    private void Reset()
    {
        _requestKey = "Emit_signIn";
        _answerKey = "sucsessedSignIn";
    }

    private void Awake()
    {
        _registrationButton.interactable = false;
        _registrationButton.onClick.AddListener(Registration);
        _login.onValueChanged.AddListener(UpdateAutoButton);
        _eMail.onValueChanged.AddListener(UpdateAutoButton);
        _password.onValueChanged.AddListener(UpdateAutoButton);
    }

    private void OnDestroy()
    {
        _registrationButton.onClick.RemoveAllListeners();
    }

    private void Registration()
    {
        var json = JsonUtility.ToJson(new ClientSignIN(
            _login.text, _eMail.text, _password.text));
        _eMail.text = "";
        _login.text = "";
        _password.text = "";
        _switcher.SwitchMenu(MenuType.None);
        OnRegistration.Invoke(_answerKey, MessageData.JsonMessange(_requestKey, json));
    }

    private void UpdateAutoButton(string text)
    {
        _registrationButton.interactable = DataFillter.CheakLogin(_login.text);
        _registrationButton.interactable &= DataFillter.CheakPassword(_password.text);
        _registrationButton.interactable &= DataFillter.CheakPassword(_eMail.text);
    }

}
