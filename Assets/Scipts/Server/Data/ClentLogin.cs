namespace Server
{
    [System.Serializable]
    public class ClientLogin
    {
        public string name;
        public string password;

        public ClientLogin(string login, string password)
        {
            this.name = login;
            this.password = password;
        }
    }
}