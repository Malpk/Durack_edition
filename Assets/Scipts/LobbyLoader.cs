using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyLoader : MonoBehaviour
{
    [SerializeField] private int _lobbyId;
    [SerializeField] private string _key = "";

    public void Load(string data)
    {
        PlayerPrefs.SetString(_key, data);
        SceneManager.LoadScene(_lobbyId);
    }
}
