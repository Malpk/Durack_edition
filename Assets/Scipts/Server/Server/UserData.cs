namespace Server
{
    public struct UserData
    {
        public string token;
        public uint UserID;
        public uint RoomID;

        public UserData(Client.UserData data)
        {
            token = data.Token;
            UserID = data.ID;
            RoomID = 0;
        }
    }
}