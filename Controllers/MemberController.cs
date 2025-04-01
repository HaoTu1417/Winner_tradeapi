// Decompiled with JetBrains decompiler
// Type: tradeapi.Controllers.MemberController
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Models.Dto;
using StockAdmin.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Identity.Data;
using tradeapi.Business;
using tradeapi.Cache;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models;
using tradeapi.Models.Dto;
using tradeapi.Models.Member;
using tradeapi.Utility;
using tradeapi.Validates;
using tradeapi2.Common;
using tradeapi2.Models.Member;
using tradeApi2.Models.Member;
using RegisterRequest = tradeapi.Models.Member.RegisterRequest;
using ResetPasswordRequest = tradeapi.Models.Member.ResetPasswordRequest;

#nullable enable
namespace tradeapi.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class MemberController : ApiController
  {
    private readonly VerifyBiz _verifyBiz;
    public MemberController(VerifyBiz verifyBiz)
    {
      _verifyBiz = verifyBiz;
    }

    private string GetDevice()
    {
      try
      {
        return this.Request.Headers["User-Agent"].ToString();
      }
      catch (Exception ex)
      {
        LogLib.Warn("[MemberController][GetDevice]" + ex.Message);
        return "";
      }
    }

    private bool LanguageAuth(string language)
    {
      List<MutilangSubjectDto> all = MutilangSubjectService.FindAll();
      if (all == null || !all.Any<MutilangSubjectDto>((Func<MutilangSubjectDto, bool>) (x => x.lang.ToUpper().Equals(language.ToUpper()))))
        throw new AppException(1217, "incorrect_language");
      return true;
    }

    [HttpPost("verifymailcode")]
    public async Task<APIResponse<VerifyMailCodeResponse>> SetVerifyMailCode(
      VerifyMailCodeRequest req)
    {
      try
      {
        VerifyMailCodeValidator validator = new VerifyMailCodeValidator();
        validator.ValidateAndThrow<VerifyMailCodeRequest>(req);
        validator.CheckMember(req.email);
        VerifyBiz.MailSendCode(req.email, VerifyBiz.SetMailVerityCode(req.email), req.lang);
        return APIResponse<VerifyMailCodeResponse>.Ok(new VerifyMailCodeResponse());
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MemberController][SetVerifyMailCode]" + ex.Message);
        return APIResponse<VerifyMailCodeResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("resetpwdverifymailcode")]
    public async Task<APIResponse> ResetPWDVerifyMailCode(VerifyMailCodeRequest req)
    {
      try
      {
        VerifyMailCodeValidator validator = new VerifyMailCodeValidator();
        validator.ValidateAndThrow<VerifyMailCodeRequest>(req);
        if (validator.CheckMailInMember(req.email))
          VerifyBiz.MailSendCode(req.email, VerifyBiz.SetMailVerityCode(req.email), req.lang);
        return APIResponse.Ok((object) null);
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MemberController][ResetPWDVerifyMailCode]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("registerphone")]
    public async Task<APIResponse> Register1(ReqString req_str)
    {
      MemberController memberController = this;
      try
      {
        RegisterRequest registerRequest = JsonSerializer.Deserialize<RegisterRequest>(DecryptTool.DecryptByAES(req_str.req_string));

        if (registerRequest != null && !string.IsNullOrEmpty(registerRequest.lang))
          memberController.lang = registerRequest.lang;
        RegisterValidator validator = new RegisterValidator();
        registerRequest.time_stamp = new long?(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        validator.ValidateAndThrow<RegisterRequest>(registerRequest);
        validator.DbAuth(registerRequest);
        using (TransactionScope transactionScope = new TransactionScope())
        {
          MemberBiz.RegisterMember(registerRequest, memberController.GetIp());
          transactionScope.Complete();
          return APIResponse.Ok((object) null, "注册成功");
        }
      }
      catch (AppException ex)
      {
        LogLib.Error((Exception) ex);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(memberController.lang));
      }
    }

    [HttpPost("register")]
    public async Task<APIResponse> Register(ReqString req_str)
    {
      MemberController memberController = this;
      try
      {
        RegisterRequest registerRequest = JsonSerializer.Deserialize<RegisterRequest>(DecryptTool.DecryptByAES(req_str.req_string));
        if (registerRequest != null && !string.IsNullOrEmpty(registerRequest.lang))
          memberController.lang = registerRequest.lang;
        RegisterValidator validator = new RegisterValidator();
        validator.ValidateAndThrow<RegisterRequest>(registerRequest);
        validator.DbAuth(registerRequest);
        using (TransactionScope transactionScope = new TransactionScope())
        {
          MemberBiz.RegisterMember(registerRequest, memberController.GetIp());
          transactionScope.Complete();
          return APIResponse.Ok((object) null, "注册成功");
        }
      }
      catch (AppException ex)
      {
        LogLib.Error((Exception) ex);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(memberController.lang));
      }
    }

    [HttpPost("signin1")]
    public APIResponse<SignInResponse> SignIn1(SignInRequest input)
    {
      MemberLoginDto member_login = new MemberLoginDto()
      {
        ip = this.GetIp(),
        login_account = input.email,
        device = this.GetDevice()
      };
      try
      {
        SignInValidator validator = new SignInValidator();
        input.time_stamp = new long?(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        validator.ValidateAndThrow<SignInRequest>(input);
        this.LanguageAuth(input.lang);
        int num = validator.DbAuth(input);
        member_login.member_fk = num;
        SignInResponse signInResponse = AuthBiz.Login(new TokenModel()
        {
          member_fk = num,
          ip = this.GetIp(),
          lang = input.lang
        });
        MemberBiz.CreateLoginRecord(member_login, 1, "登入成功");
        return APIResponse<SignInResponse>.Ok(signInResponse, "登录成功");
      }
      catch (AppException ex)
      {
        MemberBiz.CreateLoginRecord(member_login, 0, this.lang);
        LogLib.Warn("[MemberController][SignIn1]" + ex.Message);
        return APIResponse<SignInResponse>.Error(ex.GetStatus(), ex.GetMessage(input.lang));
      }
    }
    
    [HttpPost("presignin")]
    public APIResponse<SignInResponse> PreSignIn(ReqString req_str)
    {
      MemberLoginDto member_login = new MemberLoginDto()
      {
        ip = "",
        login_account = "",
        device = "",
        create_time = DateTime.UtcNow
      };
      try
      {
        SignInRequest signInRequest = JsonSerializer.Deserialize<SignInRequest>(DecryptTool.DecryptByAES(req_str.req_string));
        if (signInRequest != null && !string.IsNullOrEmpty(signInRequest.lang))
          this.lang = signInRequest.lang;
        member_login.ip = this.GetIp();
        member_login.login_account = signInRequest.email;
        member_login.device = this.GetDevice();
        PresignInValidator validator = new PresignInValidator();
        validator.ValidateAndThrow<SignInRequest>(signInRequest);
        this.LanguageAuth(signInRequest.lang);
        int num = validator.DbAuth(signInRequest);
        // username và password đã đúng.
        var logins = MemberLoginServices.GetLoginByDay(DateTime.Now, num);
        
        
        // member_login.member_fk = num;
        SignInResponse signInResponse = new SignInResponse();


        if (logins != null && logins.Count > 0)
        {
          signInResponse = AuthBiz.Login(new TokenModel()
          {
            member_fk = num,
            ip = this.GetIp(),
          
          });
          MemberBiz.CreateLoginRecord(member_login, 1, "登入成功");
        }
        
        return APIResponse<SignInResponse>.Ok(signInResponse, "登录检查成功");
      }
      catch (AppException ex)
      {
        //MemberBiz.CreateLoginRecord(member_login, 0, this.lang);
        LogLib.Warn("[MemberController][PreSignIn]" + ex.Message);
        return APIResponse<SignInResponse>.Error(ex.GetStatus(), ex.GetMessage(this.lang));
      }
    }


    [HttpPost("signin")]
    public APIResponse<SignInResponse> SignIn(ReqString req_str)
    {
      MemberLoginDto member_login = new MemberLoginDto()
      {
        ip = "",
        login_account = "",
        device = "",
        create_time = DateTime.UtcNow
      };
      try
      {
        SignInRequest signInRequest = JsonSerializer.Deserialize<SignInRequest>(DecryptTool.DecryptByAES(req_str.req_string));
        if (signInRequest != null && !string.IsNullOrEmpty(signInRequest.lang))
          this.lang = signInRequest.lang;
        member_login.ip = this.GetIp();
        member_login.login_account = signInRequest.email;
        member_login.device = this.GetDevice();
        SignInValidator validator = new SignInValidator();
        validator.ValidateAndThrow<SignInRequest>(signInRequest);
        this.LanguageAuth(signInRequest.lang);
        int num = validator.DbAuth(signInRequest);
        member_login.member_fk = num;
        SignInResponse signInResponse = AuthBiz.Login(new TokenModel()
        {
          member_fk = num,
          ip = this.GetIp(),
          
        });
        VerifyBiz.CheckPhoneVerifyCode(signInRequest.phoneNumber,signInRequest.verify_phone);
        MemberBiz.CreateLoginRecord(member_login, 1, "登入成功");
        return APIResponse<SignInResponse>.Ok(signInResponse, "登录成功");
      }
      catch (AppException ex)
      {
        MemberBiz.CreateLoginRecord(member_login, 0, this.lang);
        LogLib.Warn("[MemberController][SignIn]" + ex.Message);
        return APIResponse<SignInResponse>.Error(ex.GetStatus(), ex.GetMessage(this.lang));
      }
    }

    [HttpPost("signout")]
    public APIResponse LogOut(LangRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        AuthBiz.Logout(token);
        return APIResponse.Ok((object) "");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MemberController][LogOut]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("resetpassword")]
    public APIResponse ResetPassword(ReqString req_str)
    {
      try
      {
        TokenModel token = this.GetToken();
        ResetPasswordRequest resetPasswordRequest = JsonSerializer.Deserialize<ResetPasswordRequest>(DecryptTool.DecryptByAES(req_str.req_string));
        if (resetPasswordRequest != null && !string.IsNullOrEmpty(resetPasswordRequest.lang))
          this.lang = resetPasswordRequest.lang;
        ResetPasswordValidator validator = new ResetPasswordValidator();
        validator.ValidateAndThrow<ResetPasswordRequest>(resetPasswordRequest);
        validator.DbAuth(token, resetPasswordRequest);
        MemberBiz.DbResetPassword(resetPasswordRequest.newpasswd, token.member_fk);
        return APIResponse.Ok((object) null);
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MemberController][ResetPassword]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(this.lang));
      }
    }

    [HttpPost("resetphonenumber")]
    public APIResponse ResetPhoneNumber(ReqString req_str)
    {
      try
      {
        TokenModel token = this.GetToken();
        ResetPhoneRequest resetPhoneRequest = JsonSerializer.Deserialize<ResetPhoneRequest>(DecryptTool.DecryptByAES(req_str.req_string));
        if (resetPhoneRequest != null && !string.IsNullOrEmpty(resetPhoneRequest.lang))
          this.lang = resetPhoneRequest.lang;
        ResetPhoneNumberValidator validator = new ResetPhoneNumberValidator();
        validator.ValidateAndThrow<ResetPhoneRequest>(resetPhoneRequest);
        validator.DbAuth(token, resetPhoneRequest.password);
        MemberBiz.DbResetPhoneNumber(resetPhoneRequest, token.member_fk);
        return APIResponse.Ok((object) "");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MemberController][ResetPhoneNumber]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(this.lang));
      }
    }

    [HttpPost("verifyidentity")]
    public APIResponse VerifyIdentity(VerifyIdentityRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        new IdentityVerifyValidator().ValidateAndThrow<VerifyIdentityRequest>(req);
        MemberServices.UploadIdentityVerification(req, token.member_fk);
        MemberBiz.UpdateReviewMemberCount();
        return APIResponse.Ok((object) null, "實名認證上傳成功");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MemberController][VerifyIdentity]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("gettask")]
    public APIResponse<List<GetTaskResponse>> GetTask(LangRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<List<GetTaskResponse>>.Ok(MemberBiz.GetTaskRes(token, req.lang));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MemberController][GetTask]" + ex.Message);
        return APIResponse<List<GetTaskResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    
    [HttpPost("passwordapplyphone")]
    public APIResponse PasswordApplyPhone(ReqString req_str)
    {
      try
      {
       var currentUser =  this.GetToken();
        PasswordApplyPhoneRequest instance = JsonSerializer.Deserialize<PasswordApplyPhoneRequest>(DecryptTool.DecryptByAES(req_str.req_string));
        this.lang = instance != null && !string.IsNullOrEmpty(instance.lang) ? instance.lang : throw new AppException(1090, "error_wrong_param");
        new PasswordApplyPhoneValidator().ValidateAndThrow<PasswordApplyPhoneRequest>(instance);
       
        MemberDto memberDto = MemberServices.Find(currentUser.member_fk);
        //MemberResponse memberByPhone = MemberServices.GetByPhone(instance.phone);
        // nếu user này chưa có sdt 
        if (string.IsNullOrEmpty(memberDto.mobile))
        {
          throw new AppException(1225, "user_not_have_mobile");
        }
        
        // nếu user này có sdt 
        // có số điện thoại và nó khác với sdt input.
        if (memberDto.mobile != instance.phone)
        {
          throw new AppException(1224, "error_wrong_phone_number");
        }
        // có số điện thoại nhưng số điện thoại nhập vào lại không đúng cái hiện tại
        if (memberDto.mobile != instance.phone)
        {
          throw new AppException(1226, "phone_not_match");
        }
        
        VerifyBiz.CheckPhoneVerifyCode(instance.phone, instance.phone_verifyCode);
        MemberBiz.DbResetPassword(instance.newpasswd, memberDto.pk);
        return APIResponse.Ok((object) null, "申请变更密码成功");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MemberController][PasswordApply]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(this.lang));
      }
    }
    
    [HttpPost("passwordapply")]
    public APIResponse PasswordApply(ReqString req_str)
    {
      try
      {
        this.GetToken();
        PasswordApplyRequest instance = JsonSerializer.Deserialize<PasswordApplyRequest>(DecryptTool.DecryptByAES(req_str.req_string));
        this.lang = instance != null && !string.IsNullOrEmpty(instance.lang) ? instance.lang : throw new AppException(1090, "error_wrong_param");
        new PasswordApplyValidator().ValidateAndThrow<PasswordApplyRequest>(instance);
        VerifyBiz.CheckMailVerifyCode(instance.email, instance.email_verifyCode);
        MemberResponse byUsernameOrEmail = MemberServices.GetByUsernameOrEmail(instance.email);
        MemberBiz.DbResetPassword(instance.newpasswd, byUsernameOrEmail.pk);
        return APIResponse.Ok((object) null, "申请变更密码成功");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MemberController][PasswordApply]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(this.lang));
      }
    }

    [HttpGet("vertrfycode")]
    public async Task<IActionResult> VertrfyCode()
    {
      MemberController memberController = this;
      string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.Combine("Image", "captcha"));
      string code = Captcha.CreateCode(dir);
      CacheQueryAsync.SelectDB(3);
      bool flag = await CacheQueryAsync.StringSetAsync("vericode_" + memberController.HttpContext.Connection.RemoteIpAddress.Text(), code, new TimeSpan?(new TimeSpan(0, 5, 0)));
      IActionResult actionResult;
      using (MemoryStream memoryStream = Captcha.BuildImage(code, Path.Combine(dir, code + ".jpg")))
        actionResult = (IActionResult) memberController.File(memoryStream.ToArray(), "image/jpeg");
      dir = (string) null;
      code = (string) null;
      return actionResult;
    }

    [HttpPost("getsetting")]
    public APIResponse<GetSettingResponse?> GetSetting(LangRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<GetSettingResponse>.Ok(MemberBiz.GetSettingById(token));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MemberController][GetSetting]" + ex.Message);
        return APIResponse<GetSettingResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    // to send sms to phone
    [HttpPost("sendsmsverify")]
    public async Task<APIResponse> SendSMSVerify(SendphoneVerifyRequest req)
    {
      try
      {
        new SendVerifyCodeSmsValidator().ValidateAndThrow<SendphoneVerifyRequest>(req);
        var code = VerifyBiz.GetVerifyCode();
        int num = await _verifyBiz.SendSMSVerityESMS(req.PhoneNumber,VerifyBiz.SetPhoneVerityCode(req.PhoneNumber)) ? 1 : 0;
        return APIResponse.Ok(null, "傳送成功");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MemberController][SendSMSVerify]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("verifysmscode")]
    public async Task<APIResponse> VerifySMSCode(VerifyPhoneCodeRequest req)
    {
      try
      {
        return APIResponse.Ok((object) await MemberBiz.VerifySMSCode(req), "驗證結果");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MemberController][VerifySMSCode]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("checkphoneauth")]
    public async Task<APIResponse> CheckPhoneAuth(CheckPhoneAuthRequest req)
    {
      try
      {
        return APIResponse.Ok((object) await MemberBiz.CheckPhoneAuth(req), "驗證結果");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MemberController][CheckPhoneAuth]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("sendphoneverify")]
    public APIResponse SendPhoneVerify(SendphoneVerifyRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        new SendVerifyCodeSmsValidator().ValidateAndThrow<SendphoneVerifyRequest>(req);
        if (MemberBiz.CheckMemberSmsStatus(token.member_fk))
          throw new AppException(2410, "phone_verified");
        return APIResponse.Ok((object) null);
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MemberController][SendPhoneVerify]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("isfinishauth")]
    public APIResponse IsFinishAuth(LangRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse.Ok((object) MemberBiz.IsAuthFinished(token));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MemberController][IsFinishAuth]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("verifyphonecode")]
    public APIResponse VerifyPhoneCode(VerifyPhoneCodeRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse.Ok((object) MemberBiz.VerifyPhoneCode(token, req));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MemberController][VerifyPhoneCode]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("getnotify")]
    public APIResponse<GetNotifyResponse> GetNotify(LangRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<GetNotifyResponse>.Ok(MemberBiz.GetNotifySetting(token));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MemberController][GetNotify]" + ex.Message);
        return APIResponse<GetNotifyResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("setnotify")]
    public APIResponse SetNotify(SetNotifyRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse.Ok((object) MemberBiz.SetNotify(token, req));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MemberController][SetNotify]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("setrichboxautotransfer")]
    public APIResponse SetRichBoxAutoTransfer(SetRichBoxAutoTransferRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        MemberBiz.SetRichBoxAutoTransfer(token.member_fk, req.enable_auto_transfer);
        return APIResponse.Ok((object) "");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MemberController][SetRichBoxAutoTransfer]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("setstockchart")]
    public APIResponse SetStockChart(SetStockChartRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        MemberBiz.SetStockChart(token.member_fk, req.stock_chart_setting);
        return APIResponse.Ok((object) "");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MemberController][SetStockChart]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("getinvitationinfo")]
    public APIResponse<InvitationInfoResponse> GetInvitationInfo(LangRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        InvitationInfoResponse invitationInfoResponse;
        if (token == null)
          invitationInfoResponse = new InvitationInfoResponse()
          {
            InvitationCode = "",
            InvitationURL = ""
          };
        else
          invitationInfoResponse = MemberBiz.GetInvitationInfo(token.member_fk);
        return APIResponse<InvitationInfoResponse>.Ok(invitationInfoResponse);
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MemberController][GetInvitationInfo]" + ex.Message);
        return APIResponse<InvitationInfoResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("setpaypassword")]
    public APIResponse SetPayPassword(ReqString req_str)
    {
      try
      {
        TokenModel token = this.GetToken();
        SetPayPasswordRequest payPasswordRequest = JsonSerializer.Deserialize<SetPayPasswordRequest>(DecryptTool.DecryptByAES(req_str.req_string));
        if (payPasswordRequest != null && !string.IsNullOrEmpty(payPasswordRequest.lang))
          this.lang = payPasswordRequest.lang;
        SetPayPasswordValidator validator = new SetPayPasswordValidator();
        validator.ValidateAndThrow<SetPayPasswordRequest>(payPasswordRequest);
        validator.DbAuth(payPasswordRequest, token);
        MemberBiz.SetPayPassword(payPasswordRequest.newpasswd, token.member_fk);
        return APIResponse.Ok((object) null, "密码修改成功");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MemberController][SetPayPassword]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(this.lang));
      }
    }

    [HttpPost("resetpaypassword")]
    public APIResponse ReSetPayPassword(ReqString req_str)
    {
      try
      {
        TokenModel token = this.GetToken();
        ResetPayPasswordRequest payPasswordRequest = JsonSerializer.Deserialize<ResetPayPasswordRequest>(DecryptTool.DecryptByAES(req_str.req_string));
        if (payPasswordRequest != null && !string.IsNullOrEmpty(payPasswordRequest.lang))
          this.lang = payPasswordRequest.lang;
        ResetPayPasswordValidator validator = new ResetPayPasswordValidator();
        validator.ValidateAndThrow<ResetPayPasswordRequest>(payPasswordRequest);
        validator.DbAuth(token, payPasswordRequest);
        MemberBiz.SetPayPassword(payPasswordRequest.newpasswd, token.member_fk);
        return APIResponse.Ok((object) null, "密码修改成功");
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MemberController][ReSetPayPassword]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(this.lang));
      }
    }

    [HttpPost("setmemberlang")]
    public APIResponse SetMemberLang(LangRequest req)
    {
      try
      {
        MemberBiz.SetMemberLang(this.GetToken().member_fk, req.lang);
        return APIResponse.Ok((object) null);
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MemberController][SetMemberLang]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(this.lang));
      }
    }
  }
}
