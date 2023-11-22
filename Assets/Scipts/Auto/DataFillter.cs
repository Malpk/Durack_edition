using UnityEngine;
using System.Text.RegularExpressions;

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

    public static bool CheakEmail(string email)
    {
        var regex = new Regex(@"(\w*)\@(\w*)\.(\w*)");
        return regex.IsMatch(email);
    }
}
