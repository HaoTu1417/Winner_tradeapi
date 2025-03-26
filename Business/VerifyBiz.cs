// Decompiled with JetBrains decompiler
// Type: tradeapi.Business.VerifyBiz
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using tradeapi.Cache;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Info;
using tradeapi.Models.Member;
using tradeapi.Utility;
using tradeApi2.Models;

#nullable enable
namespace tradeapi.Business
{
    public class VerifyBiz
    {
        private readonly EsmsConfig _config;

        public VerifyBiz(IOptions<EsmsConfig> config)
        {
            _config = config.Value;
        }


        public static async Task MailSendCode(string email, string verifyCode, string lang)
        {
            DocResponse docByCid = InfoBiz.GetDocByCid("verity", lang);
            string body = docByCid.content.Replace("{0}", verifyCode);
            await Email.mailapiAsync(email, docByCid.title, body);
        }

        public async Task<bool> SendSMSVerityESMS(string phoneNumber,string verificationCode)
        {
            using (var httpClient = new HttpClient())
            {
                var requestBody = new
                {
                    ApiKey = _config.ApiKey,
                    SecretKey = _config.SecretKey,
                    Content = $"{verificationCode} la ma xac minh dang ky Baotrixemay cua ban",
                    Phone = phoneNumber,
                    Brandname = "Baotrixemay",
                    SmsType = "2",
                    IsUnicode = "0",
                    campaignid = "Cảm ơn sau mua hàng tháng 7",
                    RequestId = Guid.NewGuid().ToString(),
                    CallbackUrl = _config.CallbackUrl
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response;
                string responseContent;

                try
                {
                    response = await httpClient.PostAsync(
                        "https://rest.esms.vn/MainService.svc/json/SendMultipleMessage_V4_post_json/", content);
                    responseContent = await response.Content.ReadAsStringAsync();

                    // Log the response
                    LogLib.Info($"[eSMS Response] StatusCode: {response.StatusCode}, Body: {responseContent}");
                }
                catch (Exception ex)
                {
                    throw new AppException(2416, $"Request failed: {ex.Message}");
                }

                // Parse the response JSON
                try
                {
                    var jsonDoc = JsonDocument.Parse(responseContent);
                    var root = jsonDoc.RootElement;

                    string codeResult = root.GetProperty("CodeResult").GetString();
                   

                    if (codeResult != "100")
                    {
                        string errorMessage = root.GetProperty("ErrorMessage").GetString();
                        throw new AppException(2416,
                            $"eSMS error: CodeResult = {codeResult}, Message = {errorMessage}");
                    }
                }
                catch (JsonException ex)
                {
                    throw new AppException(2416, $"Invalid JSON response from eSMS: {ex.Message}");
                }

                return true;
            }
        }


        public static string GetVerifyCode() => new Random().Next(0, 10000).ToString("D4");

        public static string SetMailVerityCode(string email)
        {
            try
            {
                string verifyCode = VerifyBiz.GetVerifyCode();
                CacheQuery.SelectDB(4);
                CacheQuery.StringSet(email, verifyCode, new TimeSpan?(TimeSpan.FromMinutes(10.0)));
                return verifyCode;
            }
            catch (Exception ex)
            {
                LogLib.Error("[VerifyBiz][SetMailVerityCode]" + ex.Message);
                throw new AppException(1020, "redis_exception");
            }
        }

        public static void CheckMailVerifyCode(string mail, string code)
        {
            CacheQuery.SelectDB(4);
            if (!CacheQuery.KeyExists(mail))
                throw new AppException(1215, "verification_code_not_received");
            if (CacheQuery.StringGet(mail) != code)
                throw new AppException(1212, "incorrect_verification_code");
        }

        public static string SetPhoneVerityCode(string phone)
        {
            try
            {
                string verifyCode = VerifyBiz.GetVerifyCode();
                CacheQuery.SelectDB(4);
                CacheQuery.StringSet(phone, verifyCode, new TimeSpan?(TimeSpan.FromMinutes(10.0)));
                return verifyCode;
            }
            catch (Exception ex)
            {
                LogLib.Error("[VerifyBiz][SetPhoneVerityCode]" + ex.Message);
                throw new AppException(1020, "redis_exception");
            }
        }

        public static void CheckpohoneVerifyCode(string pohone, string code)
        {
            CacheQuery.SelectDB(4);
            if (!CacheQuery.KeyExists(pohone))
                throw new AppException(1215, "verification_code_not_received");
            if (CacheQuery.StringGet(pohone) != code)
                throw new AppException(1212, "incorrect_verification_code");
        }
    }
}