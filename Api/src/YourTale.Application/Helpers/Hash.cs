using System.Security.Cryptography;
using System.Text;

namespace YourTale.Application.Helpers;

public static class Hash
{
    
    public static string Md5Hash(string input)
    {
        var data = MD5.HashData(Encoding.UTF8.GetBytes(input));
        var sBuilder = new StringBuilder();

        foreach (var t in data)
        {
            sBuilder.Append(t.ToString("x2"));
        }

        return sBuilder.ToString();
    }

}