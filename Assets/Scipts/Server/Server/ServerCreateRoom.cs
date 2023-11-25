[System.Serializable]
public struct ServerCreateRoom
{
    public uint RoomID;

    public string token;

    public int isPrivate;

    public string key;

    public uint bet;

    public uint cards;

    public int type;

    public int maxPlayers;

    public uint roomOwner;
}