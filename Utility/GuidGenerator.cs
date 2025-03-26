namespace tradeApi2.Utility;

using System;
using System.Security.Cryptography;
using System.Text;


public class GuidGenerator
{
    public static Guid CreateDeterministicGuid(string input)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return new Guid(hash);
        }
    }
}