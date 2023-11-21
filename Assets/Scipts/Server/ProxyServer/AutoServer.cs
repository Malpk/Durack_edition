using System.Collections.Generic;
using UnityEngine;

public class AutoServer : MonoBehaviour
{
    [SerializeField] private List<ClientSignIN> _users;
    [Header("Reference")]
    [SerializeField] private AutoMenu _autoMenu;
    [SerializeField] private RegistrationMenu _regisration;

    public event System.Action<string> OnAcess;
    public event System.Action<string> OnFail;

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
        if (json != "" && json != null)
        {
            var data = JsonUtility.FromJson<ClientSignIN>(json);
            _users.Add(data);
            OnAcess.Invoke(key);
        }
        else
        {
            OnFail?.Invoke(key);
        }
    }

    private void OnAuto(string key, string json)
    {
        if (json != "" && json != null)
        {
            var data = JsonUtility.FromJson<ClientLogin>(json);
            if (GetPlayer(data))
            {
                OnAcess.Invoke(key);
                return;
            }
        }
        OnFail?.Invoke(key);
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
