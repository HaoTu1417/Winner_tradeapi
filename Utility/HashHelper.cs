using System.Security.Cryptography;
using System.Text;

namespace tradeApi2.Utility;

public class HashHelper
{
    public static string ToMd5String(string input)
    {
        using MD5 md5 = MD5.Create();
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        // Chuyển mảng byte thành chuỗi hex
        StringBuilder sb = new StringBuilder();
        foreach (byte b in hashBytes)
        {
            sb.Append(b.ToString("x2")); // "x2" để format thành hex, chữ thường
        }

        return sb.ToString();
    }
}