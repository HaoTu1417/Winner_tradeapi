// Decompiled with JetBrains decompiler
// Type: tradeapi.Utility.DecryptTool
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using tradeapi.Common;

#nullable enable
namespace tradeapi.Utility
{
  public class DecryptTool
  {
    public static string _KEY = "1234567890000000";
    public static string _AES_IV = "1234567890000000";

    public static string EncryptByAES(string input)
    {
      if (string.IsNullOrWhiteSpace(input))
        return input;
      using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
      {
        rijndaelManaged.Mode = CipherMode.CBC;
        rijndaelManaged.Padding = PaddingMode.PKCS7;
        rijndaelManaged.FeedbackSize = 128;
        rijndaelManaged.Key = Encoding.UTF8.GetBytes(DecryptTool._KEY);
        rijndaelManaged.IV = Encoding.UTF8.GetBytes(DecryptTool._AES_IV);
        ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write))
          {
            using (StreamWriter streamWriter = new StreamWriter((Stream) cryptoStream))
              streamWriter.Write(input);
            return Convert.ToBase64String(memoryStream.ToArray());
          }
        }
      }
    }

    public static string DecryptByAES(string input)
    {
      try
      {
        if (string.IsNullOrWhiteSpace(input))
          return input;
        byte[] buffer = Convert.FromBase64String(input);
        using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
        {
          rijndaelManaged.Mode = CipherMode.CBC;
          rijndaelManaged.Padding = PaddingMode.PKCS7;
          rijndaelManaged.FeedbackSize = 128;
          rijndaelManaged.Key = Encoding.UTF8.GetBytes(DecryptTool._KEY);
          rijndaelManaged.IV = Encoding.UTF8.GetBytes(DecryptTool._AES_IV);
          ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);
          using (MemoryStream memoryStream = new MemoryStream(buffer))
          {
            using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Read))
            {
              using (StreamReader streamReader = new StreamReader((Stream) cryptoStream))
                return streamReader.ReadToEnd();
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw new AppException(210, "illegal_operation");
      }
    }
  }
}
