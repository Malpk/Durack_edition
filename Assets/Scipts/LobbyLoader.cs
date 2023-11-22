using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyLoader : MonoBehaviour
{
    [SerializeField] private int _lobbyId;
    [SerializeField] private string _key = "";

    public void Load(MessageData data)
    {
        PlayerPrefs.SetString(_key, JsonUtility.ToJson(data));
        SceneManager.LoadScene(_lobbyId);
    }
}
