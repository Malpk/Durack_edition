[System.Serializable]
public class ClientSignIN
{
    public string name;
    public string email;
    public string password;

    public ClientSignIN(string name, string email, string password)
    {
        this.name = name;
        this.email = email;
        this.password = password;
    }
}