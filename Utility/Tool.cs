// Decompiled with JetBrains decompiler
// Type: tradeapi.Utility.Tool
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using tradeapi.Common;
using tradeapi.Models.Dto;
using tradeapi.Services;

#nullable enable
namespace tradeapi.Utility
{
  public static class Tool
  {
    private static Random _random = new Random();
    private const string _chars = "0123456789";
    private static List<string> excludeIp = new List<string>()
    {
      "127.0.0.1",
      "localhost",
      "::1"
    };
    private static TimeZoneInfo vn_tz = TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh");
    private static TimeZoneInfo us_tz = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");

    public static string ToJson(object obj) => JsonConvert.SerializeObject(obj);

    public static T FromJson<T>(string json) => JsonConvert.DeserializeObject<T>(json.Text());

    public static string ReplaceWithSpecialChar(
      string value,
      int startLen = 2,
      int endLen = 2,
      char specialChar = '*')
    {
      switch (value.Length)
      {
        case 0:
        case 1:
        case 2:
          return "***";
        case 3:
          return value.Substring(0, 1) + "**";
        case 4:
          return value.Substring(0, 1) + "**" + value.Substring(3, 1);
        default:
          int startIndex = value.Length - 2;
          return value.Substring(0, 2) + "***" + value.Substring(startIndex, 2);
      }
    }

    public static object? FromJson(string json) => JsonConvert.DeserializeObject(json);

    public static bool IsValidImageFormat(string path)
    {
      string lowerInvariant = Path.GetExtension(path).ToLowerInvariant();
      return lowerInvariant == ".jpg" || lowerInvariant == ".jpeg" || lowerInvariant == ".png" || lowerInvariant == ".gif" || lowerInvariant == ".bmp";
    }

    public static string MakeRandomNo(int length)
    {
      char[] chArray = new char[length];
      for (int index = 0; index < chArray.Length; ++index)
        chArray[index] = "0123456789"[Tool._random.Next("0123456789".Length) % "0123456789".Length];
      return new string(chArray);
    }

    public static string GetCountryByIp(string ip)
    {
      try
      {
        if (Tool.excludeIp.Contains(ip))
          return (string) null;
        using (DatabaseReader databaseReader = new DatabaseReader("GeoLite2-Country.mmdb"))
        {
          if (string.IsNullOrEmpty(ip))
            return "未知";
          if (ip == "::1")
            return "本地";
          CountryResponse countryResponse = databaseReader.Country(ip);
          return countryResponse == null ? "未知" : countryResponse.Country.Names["zh-CN"];
        }
      }
      catch (AppException ex)
      {
        throw new AppException("請求:" + ip + "，錯誤:" + ex.Message);
      }
    }

    public static string AddNumberSeparation(Decimal num)
    {
      string str1 = num.ToString();
      string str2 = "";
      int length = str1.Length;
      for (int index = length - 1; index >= 0; --index)
      {
        if (str1[index] == '-')
          return "-" + str2;
        if (index != length - 1 && (length - index - 1) % 3 == 0)
          str2 = "." + str2;
        str2 = new ReadOnlySpan<char>(str1[index]).ToString() +  str2;
      }
      return str2;
    }

    public static string MakeTradeMoneyRecordInfo(int temp_id, string lang, object[] data)
    {
      string str1 = "";
      TradeTemplateDto byTempId = TradeTemplateService.GetByTempId(temp_id, lang);
      if (byTempId != null)
        str1 = byTempId.template;
      for (int index = 0; index < data.Length; ++index)
      {
        string str2 = str1;
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 1);
        interpolatedStringHandler.AppendLiteral("#");
        interpolatedStringHandler.AppendFormatted<int>(index);
        interpolatedStringHandler.AppendLiteral("#");
        string stringAndClear = interpolatedStringHandler.ToStringAndClear();
        string newValue = Convert.ToString(data[index]);
        str1 = str2.Replace(stringAndClear, newValue);
      }
      return str1;
    }

    public static (string, string) MakeWalletRecordInfo(int temp_id, string lang, object[] data)
    {
      string str1 = "";
      StringBuilder stringBuilder1 = new StringBuilder();
      WalletTemplateDto byTempId = WalletTemplateService.GetByTempId(temp_id, lang);
      if (byTempId != null)
        str1 = byTempId.template;
      for (int index = 0; index < data.Length; ++index)
      {
        string str2 = str1;
        DefaultInterpolatedStringHandler interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(2, 1);
        interpolatedStringHandler1.AppendLiteral("#");
        interpolatedStringHandler1.AppendFormatted<int>(index);
        interpolatedStringHandler1.AppendLiteral("#");
        string stringAndClear = interpolatedStringHandler1.ToStringAndClear();
        string newValue = Convert.ToString(data[index]);
        str1 = str2.Replace(stringAndClear, newValue);
        if (index > 0)
          stringBuilder1.Append('|');
        StringBuilder stringBuilder2 = stringBuilder1;
        StringBuilder stringBuilder3 = stringBuilder2;
        StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler2 = new StringBuilder.AppendInterpolatedStringHandler(0, 1, stringBuilder2);
        interpolatedStringHandler2.AppendFormatted<object>(data[index]);
        ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler2;
        stringBuilder3.Append(ref local);
      }
      return (str1, stringBuilder1.ToString());
    }

    public static string handleVNDCurrency(string num_string)
    {
      int length1 = num_string.IndexOf('.');
      if (length1 != -1)
        num_string = num_string.Substring(0, length1);
      string str = "";
      int length2 = num_string.Length;
      for (int index = length2 - 1; index >= 0; --index)
      {
        if (num_string[index] == '-')
          return "-" + str;
        if (index != length2 - 1 && (length2 - index - 1) % 3 == 0)
          str = "." + str;
        str = new ReadOnlySpan<char>(num_string[index]).ToString() + str;
      }
      return str;
    }

    public static string handleUSDCurrency(string num_string)
    {
      string str1 = "";
      int length1 = num_string.Length;
      int num = num_string.IndexOf('.');
      string str2 = "";
      if (num != -1)
      {
        str2 = num_string.Substring(num, num_string.Length - num);
        num_string = num_string.Substring(0, num);
      }
      int length2 = num_string.Length;
      for (int index = length2 - 1; index >= 0; --index)
      {
        if (num_string[index] == '-')
          return "-" + str1;
        if (index != length2 - 1 && (length2 - index - 1) % 3 == 0)
          str1 = "," + str1;
        str1 = new ReadOnlySpan<char>(num_string[index]).ToString() + str1;
      }
      return str1 + str2;
    }

    public static string AddNumberSeparation(Decimal? num, string currency)
    {
      if (!num.HasValue)
        return "";
      string num_string = num.ToString();
      switch (currency)
      {
        case "VND":
          return Tool.handleVNDCurrency(num_string);
        case "USD":
          return Tool.handleUSDCurrency(num_string);
        default:
          return Tool.handleVNDCurrency(num_string);
      }
    }

    public static Decimal Round(Decimal value, string currency)
    {
      int decimals = currency == "VND" || currency == "TWD" ? 0 : 2;
      return Math.Round(value, decimals);
    }

    public static string GetRandomNum(int length)
    {
      Random random = new Random();
      string randomNum = "";
      for (int index = 0; index < length; ++index)
      {
        int num = random.Next(0, 10);
        randomNum += num.ToString();
      }
      return randomNum;
    }

    public static string GetCurrencyByMarket(string market)
    {
      SysMarketDto sysMarketDto = SysMarketService.Find(market);
      return sysMarketDto == null ? "VND" : sysMarketDto.currency;
    }

    public static DateTime GetTime(string market)
    {
      TimeZoneInfo destinationTimeZone;
      switch (market)
      {
        case "US":
          destinationTimeZone = Tool.us_tz;
          break;
        case "VN":
          destinationTimeZone = Tool.vn_tz;
          break;
        default:
          throw new Exception("Unkown market");
      }
      return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, destinationTimeZone);
    }

    public static DateTime ConvertTimeToUtc(string market, DateTime time)
    {
      TimeZoneInfo timeZoneInfo;
      switch (market)
      {
        case "US":
          timeZoneInfo = Tool.us_tz;
          break;
        case "VN":
          timeZoneInfo = Tool.vn_tz;
          break;
        default:
          throw new Exception("Unkown market");
      }
      TimeZoneInfo sourceTimeZone = timeZoneInfo;
      time = DateTime.SpecifyKind(time, DateTimeKind.Unspecified);
      return TimeZoneInfo.ConvertTimeToUtc(time, sourceTimeZone);
    }

    public static DateTime ConvertTimeFromUtc(string market, DateTime Utctime)
    {
      TimeZoneInfo timeZoneInfo;
      switch (market)
      {
        case "US":
          timeZoneInfo = Tool.us_tz;
          break;
        case "VN":
          timeZoneInfo = Tool.vn_tz;
          break;
        default:
          throw new Exception("Unkown market");
      }
      TimeZoneInfo destinationTimeZone = timeZoneInfo;
      return TimeZoneInfo.ConvertTimeFromUtc(Utctime, destinationTimeZone);
    }

    public static bool ToBool(string number)
    {
      return !(number == "0") && !(number.ToLower() == "false") && !(number.ToLower() == "f");
    }
  }
}
