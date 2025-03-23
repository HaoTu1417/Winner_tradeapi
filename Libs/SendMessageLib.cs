// Decompiled with JetBrains decompiler
// Type: tradeapi.Libs.SendMessageLib
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using Models.Dto;
using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using tradeapi.Common;

#nullable enable
namespace tradeapi.Libs
{
  public class SendMessageLib
  {
    public static void Send(int member_pk, int temp_id, params object[] list)
    {
      MemberDto memberDto = MemberServices.Find(member_pk);
      MessageTemplateDto byTemplateId = MessageTemplateService.FindByTemplateId(temp_id, memberDto.lang);
      string str1 = "";
      if (byTemplateId != null)
      {
        str1 = byTemplateId.template.ToString();
        for (int index = 0; index < list.Length; ++index)
        {
          if (list[index] != null)
          {
            string str2 = str1;
            DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 1);
            interpolatedStringHandler.AppendLiteral("#");
            interpolatedStringHandler.AppendFormatted<int>(index);
            interpolatedStringHandler.AppendLiteral("#");
            string stringAndClear = interpolatedStringHandler.ToStringAndClear();
            string newValue = list[index].ToString();
            str1 = str2.Replace(stringAndClear, newValue);
          }
        }
      }
      DateTime utcNow = DateTime.UtcNow;
      MessageRecordService.FindPkAfterInsert(new MessageRecordDto()
      {
        isbatch = false,
        receiver_table = AccountTypeEnum.Member,
        receiver_fk = member_pk,
        sender_table = AccountTypeEnum.AdminUser,
        sender_fk = memberDto.admin_user_fk,
        title = byTemplateId != null ? byTemplateId.title : "",
        info = str1,
        read_status = MessageReadStatusEnum.Unread,
        send_status = MessageSendStatus.Sented,
        send_type = MessageTransTypeEnum.Internal,
        create_time = utcNow,
        read_time = new DateTime?(),
        sent_time = utcNow,
        classify = 1
      });
    }

    public static bool SendPhoneMessage(
      int member_pk,
      string phone_number,
      int temp_id,
      params object[] list)
    {
      try
      {
        MemberDto memberDto = MemberServices.Find(member_pk);
        MessageTemplateDto byTemplateId = MessageTemplateService.FindByTemplateId(temp_id, memberDto.lang);
        string str1 = "";
        if (byTemplateId != null)
        {
          str1 = byTemplateId.template.ToString();
          for (int index = 0; index < list.Length; ++index)
          {
            if (list[index] != null)
            {
              string str2 = str1;
              DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 1);
              interpolatedStringHandler.AppendLiteral("#");
              interpolatedStringHandler.AppendFormatted<int>(index);
              interpolatedStringHandler.AppendLiteral("#");
              string stringAndClear = interpolatedStringHandler.ToStringAndClear();
              string newValue = list[index].ToString();
              str1 = str2.Replace(stringAndClear, newValue);
            }
          }
        }
        string str3 = "ruanjian888";
        string str4 = "Aa123123..";
        string str5 = Uri.EscapeDataString(phone_number);
        WebClient webClient = new WebClient();
        webClient.Credentials = CredentialCache.DefaultCredentials;
        string md5String = MD5Service.GetMD5String(str4, MD5Service.EncodingType.UTF8);
        string str6 = HttpUtility.UrlEncode(str1, Encoding.UTF8);
        string str7 = Encoding.UTF8.GetString(webClient.DownloadData("http://api.smsbao.com/wsms?u=" + str3 + "&p=" + md5String + "&m=" + str5 + "&c=" + str6));
        if (str7 != null)
        {
          switch (str7.Length)
          {
            case 1:
              if (str7 == "0")
                return true;
              break;
            case 2:
              switch (str7[1])
              {
                case '0':
                  switch (str7)
                  {
                    case "30":
                      throw new AppException(2412, "sms_password_error");
                    case "40":
                      throw new AppException(2413, "sms_account_notexist");
                    case "50":
                      throw new AppException(2417, "sms_sensitive_content");
                  }
                  break;
                case '1':
                  switch (str7)
                  {
                    case "41":
                      throw new AppException(2414, "sms_InsufficientBalance");
                    case "51":
                      throw new AppException(2418, "sms_Invalid_phone");
                    case "-1":
                      throw new AppException(2419, "sms_Invalid_phone_or_missing_data");
                  }
                  break;
                case '2':
                  if (str7 == "42")
                    throw new AppException(2415, "sms_account_expired");
                  break;
                case '3':
                  if (str7 == "43")
                    throw new AppException(2416, "sms_Ip_Address_Limit");
                  break;
              }
              break;
          }
        }
        throw new AppException(2420, "sms_unknow_errorcode");
      }
      catch
      {
        throw new AppException(900, "other_error");
      }
    }
  }
}
