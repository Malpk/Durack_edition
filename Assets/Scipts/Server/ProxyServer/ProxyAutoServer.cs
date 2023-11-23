using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using Server;

public class ProxyAutoServer : MonoBehaviour
{
    [SerializeField] private List<ClientSignIN> _users;
    [Header("Reference")]
    [SerializeField] private AutoMenu _autoMenu;
    [SerializeField] private RegistrationMenu _regisration;
    [Header("Events")]
    [SerializeField] private UnityEvent<string> _onAcess;
    [SerializeField] private UnityEvent<string> _onFail;

    private void Awake()
    {
        _autoMenu.OnAuto += OnAuto;
        _regisration.OnRegistration += OnRegistration;
    }

    private void OnDestroy()
    {
        _autoMenu.OnAuto -= OnAuto;
        _regisration.OnRegistration -= OnRegistration;
    }

    private void OnRegistration(string key, string json)
    {
        //if (json != "" && json != null)
        //{
        //    var data = JsonUtility.FromJson<ClientSignIN>(json);
        //    _users.Add(data);
        //    _onAcess?.Invoke(new MessageData(key,
        //        JsonUtility.ToJson(data)));
        //}
        //else
        //{
        //    _onFail?.Invoke(new MessageData(key, null));
        //}
    }

    private void OnAuto(string key, string json)
    {
        //if (json != "" && json != null)
        //{
        //    var data = JsonUtility.FromJson<ClientLogin>(json);
        //    if (GetPlayer(data))
        //    {
        //        _onAcess?.Invoke(data);
        //        return;
        //    }
        //}
        //_onFail?.Invoke(null);
    }

    private bool GetPlayer(ClientLogin data)
    {
        foreach (var user in _users)
        {
            if (user.name == data.name)
            {
                if (user.password == data.password)
                    return true;
            }
        }
        return false;
    }
}
