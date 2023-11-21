using UnityEngine;

public class DataFillter : MonoBehaviour
{
    private const int MINLOGINSIZE = 3;
    private const int MINPASSWORDSIZE = 6;

    public static bool CheakLogin(string login)
    {
        return login.Length >= MINLOGINSIZE;
    }

    public static bool CheakPassword(string password)
    {
        return password.Length >= MINPASSWORDSIZE;
    }
}
