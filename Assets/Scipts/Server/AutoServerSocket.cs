using UnityEngine;
using UnityEngine.Events;

public class AutoServerSocket : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private AutoMenu _auto;  //Меню авторизации
    [SerializeField] private SocketServer _scoket;
    [SerializeField] private RegistrationMenu _registration;  //Меню регистрации
    [Header("Events")]
    [SerializeField] private UnityEvent<string> _onGetMessange;

    private void Awake()
    {
        _auto.OnAuto += SendMessange;
        _registration.OnRegistration += SendMessange;
    }

    private void OnDestroy()
    {
        _auto.OnAuto -= SendMessange;
        _registration.OnRegistration -= SendMessange;
    }

    private void SendMessange(string key, string data)
    {
        _scoket.SendRequest(key, data, (string answer) => _onGetMessange.Invoke(answer));
    }

}
