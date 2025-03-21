// Decompiled with JetBrains decompiler
// Type: tradeapi.Business.MemberBiz
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using tradeapi.Cache;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models;
using tradeapi.Models.Dto;
using tradeapi.Models.Member;
using tradeapi.Services;
using UniSdk;

#nullable enable
namespace tradeapi.Business
{
  public class MemberBiz
  {
    private static string CreatePassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    public static void DbResetPassword(string newpasswd, int pk)
    {
      string password = MemberBiz.CreatePassword(newpasswd);
      if (MemberServices.ResetPassword(pk, password) == 0)
        throw new AppException(1218, "password_reset_failed");
    }

    public static void SetPayPassword(string newpasswd, int pk)
    {
      string password = MemberBiz.CreatePassword(newpasswd);
      if (MemberServices.ResetPayPassword(pk, password) == 0)
        throw new AppException(1218, "password_reset_failed");
    }

    public static void DbResetPhoneNumber(ResetPhoneRequest req, int member_fk)
    {
      if (MemberServices.ResetPhoneNumber(member_fk, req.country_code, req.phone_number) == 0)
        throw new AppException(1302, "phone_reset_failed");
    }

    public static List<GetTaskResponse> GetTaskRes(TokenModel tokenModel, string lang)
    {
      int num;
      if (tokenModel != null)
      {
        int memberFk = tokenModel.member_fk;
        num = tokenModel.member_fk;
      }
      else
        num = -1;
      int member_fk = num;
      string currency = ConfigLib.Get("wallet_currency");
      List<MemberTaskDto> allByLang = MemberTaskService.FindAllByLang(lang, currency);
      List<GetTaskResponse> taskRes = new List<GetTaskResponse>();
      List<WalletCouponRecordDto> allByMemberFk = WalletCouponRecordService.FindAllByMemberFK(member_fk);
      for (int index = 0; index < allByLang.Count; ++index)
      {
        MemberTaskDto task = allByLang[index];
        GetTaskResponse getTaskResponse = new GetTaskResponse()
        {
          id = task.sub_type.ToString(),
          title = task.title,
          content = task.content,
          reward = task.coin,
          isCompleted = tokenModel != null && allByMemberFk.Any<WalletCouponRecordDto>((Func<WalletCouponRecordDto, bool>) (record => record.sub_type == task.sub_type)),
          currency = task.currency
        };
        taskRes.Add(getTaskResponse);
      }
      return taskRes;
    }

    public static GetSettingResponse? GetSettingById(TokenModel tokenMode)
    {
      MemberDto member = MemberServices.GetMember(tokenMode.member_fk);
      if (member == null)
        return (GetSettingResponse) null;
      return new GetSettingResponse()
      {
        Avatar = member.head_img,
        Email = member.email,
        PhoneNumber = member.mobile_country + " " + member.mobile,
        IsRealNameVerified = member.id_auth == 1,
        DateFormat = member.date_format,
        AuthStatus = member.id_auth,
        stock_chart_setting = member.stock_chart_setting,
        enable_auto_transfer = member.enable_auto_transfer,
        need_withdraw_selfie = member.need_withdraw_selfie,
        AlreadySetPaypwd = !string.IsNullOrEmpty(member.paywd),
        lang = member.lang
      };
    }

    public static bool CheckMemberSmsStatus(int member_pk)
    {
      return (MemberServices.GetMember(member_pk) ?? throw new AppException(1201, "none_account")).sms_status;
    }

    public static bool CheckSMS(string mobile, ref string newCode)
    {
      VerityService verityService = new VerityService();
      VerifyResponse byEmail = verityService.GetByEmail(mobile);
      if (byEmail == null)
      {
        verityService.Update(new VerifyResponse()
        {
          email = mobile,
          send_time = DateTime.UtcNow,
          code = newCode,
          type = (byte) 2
        });
        return true;
      }
      if ((DateTime.UtcNow - byEmail.send_time).TotalMinutes <= 1.0)
        throw new AppException(2411, "wait60_seconds");
      byEmail.send_time = DateTime.UtcNow;
      byEmail.code = newCode;
      verityService.Update(byEmail);
      return true;
    }

    public static bool SendVerificationCode(
      int memeber_pk,
      SendphoneVerifyRequest request,
      string sendContent,
      string clientIp)
    {
      try
      {
        new Random().Next(0, 10000).ToString("D4");
        string str1 = "ruanjian888";
        string str2 = "Aa123123..";
        string str3 = Uri.EscapeDataString(request.CountryCode + request.PhoneNumber);
        string str4 = sendContent;
        WebClient webClient = new WebClient();
        webClient.Credentials = CredentialCache.DefaultCredentials;
        string md5String = MD5Service.GetMD5String(str2, MD5Service.EncodingType.UTF8);
        string str5 = HttpUtility.UrlEncode(str4, Encoding.UTF8);
        string str6 = Encoding.UTF8.GetString(webClient.DownloadData("http://api.smsbao.com/wsms?u=" + str1 + "&p=" + md5String + "&m=" + str3 + "&c=" + str5));
        if (str6 != null)
        {
          switch (str6.Length)
          {
            case 1:
              if (str6 == "0")
                return true;
              break;
            case 2:
              switch (str6[1])
              {
                case '0':
                  switch (str6)
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
                  switch (str6)
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
                  if (str6 == "42")
                    throw new AppException(2415, "sms_account_expired");
                  break;
                case '3':
                  if (str6 == "43")
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

    public static bool IsAuthFinished(TokenModel tokenMode) => true;

    public static bool VerifyPhoneCode(TokenModel tokenMode, VerifyPhoneCodeRequest request)
    {
      return true;
    }

    public static GetNotifyResponse GetNotifySetting(TokenModel tokenModel)
    {
      return new GetNotifyResponse()
      {
        EmailNotify = true,
        SiteMessageNotify = true,
        AccountAlertNotify = true,
        AccountMarginCallNotify = true,
        StockTransactionNotify = true,
        AccountExpiryNotify = 2,
        PromotionsNotify = true,
        DepositApprovedNotify = true,
        WithdrawalApprovedNotify = true,
        TradingAccountApprovedNotify = true
      };
    }

    public static bool SetNotify(TokenModel tokenMode, SetNotifyRequest req) => true;

    public static void SetStockChart(int member_fk, int setting)
    {
      MemberServices.SetStockChartSetting(member_fk, setting);
    }

    public static void SetRichBoxAutoTransfer(int member_fk, int setting)
    {
      MemberServices.SetRichBoxAutoTransfer(member_fk, setting);
    }

    public static InvitationInfoResponse GetInvitationInfo(int member_fk)
    {
      MemberDto memberDto = MemberServices.Find(member_fk);
      if (memberDto.invitation_code == null)
        throw new AppException(1305, "no_invitation_code");
      return new InvitationInfoResponse()
      {
        InvitationCode = memberDto.invitation_code,
        InvitationURL = " " + ConfigLib.Get("invitation_url") + "?code=" + memberDto.invitation_code,
        is_test_account = memberDto.is_test_account
      };
    }

    public static int CreateLoginRecord(MemberLoginDto member_login, int status, string remark)
    {
      member_login.status = status;
      member_login.remark = remark;
      return MemberLoginServices.FindPkAfterInsert(member_login);
    }

    public static bool CheckVerity(PasswordApplyRequest input)
    {
      VerityService verityService = new VerityService();
      VerifyResponse byEmail = verityService.GetByEmail(input.email);
      if (byEmail == null)
        throw new AppException(1215, "verification_code_not_received");
      if ((DateTime.UtcNow - byEmail.send_time).TotalMinutes > 15.0)
        throw new AppException(1216, "verification_code_expired");
      if (input.email_verifyCode != byEmail.code)
        throw new AppException(1212, "incorrect_verification_code");
      verityService.Delete(input.email);
      return true;
    }

    public static async Task<bool> SendSMSVerity(SendphoneVerifyRequest input)
    {
      UniResponse uniResponse = await new UniClient("S8X7iv7izLHCxyYDhC8M6").Otp.SendAsync((object) new
      {
        to = (input.CountryCode + input.PhoneNumber),
        signature = "flyer"
      });
      if (uniResponse.Status != 200)
        throw new AppException(2416, uniResponse.Message);
      return true;
    }

    public static async Task<bool> VerifySMSCode(VerifyPhoneCodeRequest input)
    {
      UniResponse uniResponse = await new UniClient("S8X7iv7izLHCxyYDhC8M6").Otp.VerifyAsync((object) new
      {
        to = (input.CountryCode + input.PhoneNumber),
        code = input.VerificationCode
      });
      return uniResponse.Status == 200 ? uniResponse.Valid : throw new AppException(2416, uniResponse.Message);
    }

    public static async Task<bool> CheckPhoneAuth(CheckPhoneAuthRequest input)
    {
      return MemberServices.FindPhoneExist(input.CountryCode, input.PhoneNumber);
    }

    public static void RegisterMember(RegisterRequest input, string client_ip)
    {
      int pkAfterInsert = MemberServices.FindPkAfterInsert(new MemberDto()
      {
        email = input.email,
        passwd = MemberBiz.CreatePassword(input.passwd),
        account = input.account,
        nickname = input.account,
        mobile_country = input.mobile_country,
        mobile = input.mobile,
        create_time = DateTime.UtcNow,
        create_ip = client_ip,
        last_login_ip = client_ip,
        email_status = true,
        lang = input.lang,
        country = ConfigLib.Get("app_default_lang"),
        date_format = input.country_pk == "VIETNAM" ? new int?(5) : new int?()
      });
      string invitationCode = (pkAfterInsert + 1000).ToString();
      MemberServices.UpdateIInvitationCode(pkAfterInsert, invitationCode);
      WalletService.InsertWallet(new WalletDto()
      {
        member_fk = pkAfterInsert,
        currency = ConfigLib.Get("wallet_currency"),
        freeze = 0.00M,
        anxin_balance = 0.00M,
        status = true,
        coupon = 0.00M,
        total_recharge = 0.00M,
        total_withdraw = 0.00M,
        last_update_time = DateTime.UtcNow
      });
      MemberNotifyServices.Insert(new Member_notifyDto()
      {
        member_fk = pkAfterInsert,
        EmailNotify = true,
        SiteMessageNotify = true,
        AccountAlertNotify = true,
        AccountMarginCallNotify = true,
        StockTransactionNotify = true,
        AccountExpiryNotify = 2,
        PromotionsNotify = true,
        DepositApprovedNotify = true,
        WithdrawalApprovedNotify = true,
        TradingAccountApprovedNotify = true
      });
      RecommendBusiness.RegisterInvitee(pkAfterInsert, input.invitation_code);
      MemberTaskLib.MemberTaskFinish(pkAfterInsert, 1);
    }

    public static void UpdateReviewMemberCount()
    {
      int reviewMemberCount = MemberServices.GetReviewMemberCount();
      CacheQuery.SelectDB(CacheEnum.admin);
      HomeVm redisValue = CacheQuery.StringGet<HomeVm>("HomeVm");
      redisValue.MemberVerifying = reviewMemberCount.ToString();
      CacheQuery.StringSet<HomeVm>("HomeVm", redisValue);
    }

    public static Decimal GetRichBoxRate(int member_fk) => MemberServices.GetRichBoxRate(member_fk);

    public static void SetMemberLang(int member_fk, string lang)
    {
      MemberServices.SetMemberLang(member_fk, lang);
    }
  }
}
