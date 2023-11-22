[System.Serializable]
public class MessageData
{
    public string eventType;
    public string data;

    public MessageData(string eventType, string data)
    {
        this.data = data;
        this.eventType = eventType;
    }
}