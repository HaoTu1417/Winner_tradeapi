// Decompiled with JetBrains decompiler
// Type: Extension
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Web;

#nullable enable
public static class Extension
{
  public static string Text(this object? obj) => (obj ?? (object) "").ToString() ?? "";

  public static string Left(this object? obj, int length)
  {
    if (length <= 0)
      return "";
    string str = obj.Text();
    return str.Length <= length ? str : str.Substring(0, length);
  }

  public static string Right(this object? obj, int length)
  {
    string str = obj.Text();
    return str.Length <= length ? str : str.Substring(str.Length - length);
  }

  public static bool IsEmpty(this object? obj) => obj.Text().Trim() == "";

  public static int Int(this object? obj)
  {
    System.Decimal result;
    return !System.Decimal.TryParse(obj.Text(), out result) ? 0 : (int) result;
  }

  public static uint UInt(this object? obj)
  {
    System.Decimal result;
    return !System.Decimal.TryParse(obj.Text(), out result) ? 0U : (uint) result;
  }

  public static string Int(this object? obj, string format) => obj.Int().ToString(format);

  public static System.Decimal Decimal(this object? obj)
  {
    System.Decimal result;
    return !System.Decimal.TryParse(obj.Text(), out result) ? 0M : result;
  }

  public static string Decimal(this object? obj, string format) => obj.Decimal().ToString(format);

  public static DateTime? Date(this object? obj)
  {
    DateTime result;
    return !DateTime.TryParse(obj.Text(), out result) ? new DateTime?() : new DateTime?(result);
  }

  public static string Date(this object? obj, string format)
  {
    DateTime result;
    return !DateTime.TryParse(obj.Text(), out result) ? "" : result.ToString(format);
  }

  public static string MD5(this object? obj)
  {
    return Convert.ToBase64String(System.Security.Cryptography.MD5.HashData(Encoding.UTF8.GetBytes(obj.Text())));
  }

  public static string Json(this object? obj)
  {
    return !(obj is Task) ? JsonConvert.SerializeObject(obj) : throw new InvalidOperationException("Object of type \"Task\" cannot be converted to JSON format.");
  }

  public static string JavaScriptStringEncode(this object? obj)
  {
    return HttpUtility.JavaScriptStringEncode(obj.Text());
  }

  public static string UrlEncode(this object? obj) => HttpUtility.UrlEncode(obj.Text());
}
