using UnityEngine;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

public class TableSocket : MonoBehaviour
{
    [SerializeField] private SocketServer _socket;

    private List<System.Action> _queryCards = new List<System.Action>();

    public event System.Action<Card> OnPlayerGetCard;
    public event System.Action<UserClient> OnEnemyGetCard;
    public event System.Action<ClientReady> OnReady;

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

    private void GetServer(MessageData messange)
    {
        if (messange.eventType == "clienReady")
        {
            OnReady?.Invoke(JsonConvert.
           DeserializeObject<ClientReady>(messange.data));
        }
        else
        {
            var action = GiveCard(messange);
            if (action != null)
            {
                _queryCards.Add(action);
            }
        }
    }

    private System.Action GiveCard(MessageData messange)
    {
        if (messange.eventType == "GetCard")
        {
            return () => OnPlayerGetCard?.Invoke(JsonConvert.
                    DeserializeObject<Card>(messange.data));
        }
        else if(messange.eventType == "cl_gotCard")
        {
            return () => OnEnemyGetCard?.Invoke(JsonConvert.
                    DeserializeObject<UserClient>(messange.data));
        }
        return null;
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
