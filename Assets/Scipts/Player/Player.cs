using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private string _login;

    public int ID => _id;
    public string Login => _login;

    public void BindPlayer(UserData data)
    {
        _id = data.ID;
        _login = data.Login;
    }

}
