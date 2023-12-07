using UnityEngine;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

public class TableSocket : MonoBehaviour
{
    [SerializeField] private SocketServer _socket;

    private List<System.Action> _queryCards = new List<System.Action>();
    private Dictionary<System.Action<string>, string> _actions = new Dictionary<System.Action<string>, string>();


    private void OnEnable()
    {
        _socket.OnGetMessange += GetServer;
        StartCoroutine(UpdateQuery());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        _socket.OnGetMessange -= GetServer;
    }

    public void AddAction(string answer, System.Action<string> action)
    {
        _actions.Add(action, answer);
    }

    public void SendThrowCard(RequestThrow request)
    {
        var messange = MessageData.JsonMessange("srv_Throw",
            JsonConvert.SerializeObject(request));
        Debug.Log(messange);
        _socket.SendRequest(messange);
    }
    public void SendGrabCard(Server.UserData data, System.Action<string> action)
    {
        var messange = MessageData.JsonMessange("srv_grab",
            JsonConvert.SerializeObject(data));
        _socket.SendRequest("grab", messange, action);
    }

    public void SendFoldCard(Server.UserData data, System.Action<string> action)
    {
        var messange = MessageData.JsonMessange("srv_fold",
            JsonConvert.SerializeObject(data));
        _socket.SendRequest("cl_playerFold", messange, action);
    }
    public void SendPassCard(Server.UserData data, System.Action<string> action)
    {
        var messange = MessageData.JsonMessange("srv_pass",
            JsonConvert.SerializeObject(data));
        _socket.SendRequest("cl_pass", messange, action);
    }

    public void SendBeatCard(RequestBeat request, System.Action<string> action)
    {
        var messange = MessageData.JsonMessange("srv_Throw",
            JsonConvert.SerializeObject(request));
        _socket.SendRequest("cl_BeatCard", messange, action);
    }

    private void GetServer(MessageData messange)
    {
        Debug.Log(messange.eventType);
        foreach (var request in _actions)
        {
            if (request.Value == messange.eventType)
            {
                request.Key.Invoke(messange.data);
            }
        }
    }


    private IEnumerator UpdateQuery()
    {
        while (enabled)
        {
            yield return new WaitWhile(() => _queryCards.Count == 0);
            _queryCards[0].Invoke();
            _queryCards.RemoveAt(0);
            yield return new WaitForSeconds(0.2f);
        }
    }

}
