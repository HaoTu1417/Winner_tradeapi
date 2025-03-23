// Decompiled with JetBrains decompiler
// Type: tradeapi.Business.VerifyBiz
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System;
using System.Threading.Tasks;
using tradeapi.Cache;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Info;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Business
{
    public class VerifyBiz
    {
        public static async Task MailSendCode(string email, string verifyCode, string lang)
        {
            DocResponse docByCid = InfoBiz.GetDocByCid("verity", lang);
            string body = docByCid.content.Replace("{0}", verifyCode);
            await Email.mailapiAsync(email, docByCid.title, body);
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
    }
}