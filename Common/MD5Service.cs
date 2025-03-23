// Decompiled with JetBrains decompiler
// Type: tradeapi.Common.MD5Service
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;
using System.Security.Cryptography;
using System.Text;

#nullable enable
namespace tradeapi.Common
{
  public class MD5Service
  {
    public static string GetMD5String(string str, MD5Service.EncodingType encodingType = MD5Service.EncodingType.ASCII)
    {
      MD5 md5 = MD5.Create();
      byte[] bytes;
      switch (encodingType)
      {
        case MD5Service.EncodingType.UTF8:
          bytes = Encoding.UTF8.GetBytes(str);
          break;
        case MD5Service.EncodingType.ASCII:
          bytes = Encoding.ASCII.GetBytes(str);
          break;
        default:
          return "";
      }
      byte[] hash = md5.ComputeHash(bytes);
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < hash.Length; ++index)
        stringBuilder.Append(hash[index].ToString("X2"));
      return stringBuilder.ToString();
    }

    public static string MD5Password(string password)
    {
      string empty = string.Empty;
      byte[] hash1 = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(password));
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < hash1.Length; ++index)
        stringBuilder.Append(hash1[index].ToString("X2"));
      byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
      byte[] hash2;
      using (SHA256 shA256 = SHA256.Create())
        hash2 = shA256.ComputeHash(bytes);
      return BitConverter.ToString(hash2).Replace("-", "").ToLower();
    }

    public static string Addmd5(string key)
    {
      byte[] hash = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(key));
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < hash.Length; ++index)
        stringBuilder.Append(hash[index].ToString("X2"));
      return stringBuilder.ToString();
    }

    public enum EncodingType
    {
      UTF8,
      ASCII,
    }
  }
}
