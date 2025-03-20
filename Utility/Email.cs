// Decompiled with JetBrains decompiler
// Type: tradeapi.Utility.Email
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Newtonsoft.Json;
using sendmail.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using tradeapi.Common;
using tradeapi.Libs;

#nullable enable
namespace tradeapi.Utility
{
  public static class Email
  {
    public static void Send(string to, string cc, string subject, string body)
    {
      cc = cc.Trim();
      string displayName = subject;
      string address = "yfpz3201c5@gmail.com";
      string host = "smtp.gmail.com";
      string userName = "yfpz3201c5@gmail.com";
      string password = "wfmnkyawnuarndyd";
      MailMessage message = new MailMessage(new MailAddress(address, displayName), new MailAddress(to));
      if (cc != "")
        message.CC.Add(new MailAddress(cc));
      message.Subject = subject;
      message.IsBodyHtml = true;
      message.Body = body;
      using (SmtpClient smtpClient = new SmtpClient(host))
      {
        smtpClient.Port = 587;
        smtpClient.EnableSsl = true;
        smtpClient.Timeout = 5000;
        if (userName.Trim() != "" && password.Trim() != "")
          smtpClient.Credentials = (ICredentialsByHost) new NetworkCredential(userName, password);
        smtpClient.Send(message);
      }
    }

    public static void Send(string to, string subject, string body)
    {
      Email.Send(to, "", subject, body);
    }

    public static void NewSend()
    {
      try
      {
        string host = "smtp.gmail.com";
        string str = "yfpz3201c5@gmail.com";
        string password = "wfmnkyawnuarndyd";
        SmtpClient smtpClient = new SmtpClient(host);
        smtpClient.Port = 587;
        smtpClient.EnableSsl = true;
        smtpClient.Timeout = 10000;
        smtpClient.UseDefaultCredentials = false;
        NetworkCredential networkCredential = new NetworkCredential(str, password);
        smtpClient.Credentials = (ICredentialsByHost) networkCredential;
        smtpClient.Send(new MailMessage(new MailAddress(str, "客服人員"), new MailAddress("xqcm168@gmail.com"))
        {
          Subject = "註冊認證信",
          SubjectEncoding = Encoding.UTF8,
          Body = "這是註冊認證信<br>認證碼12345<br>",
          BodyEncoding = Encoding.UTF8,
          IsBodyHtml = true
        });
      }
      catch (SmtpException ex)
      {
        throw new AppException("SmtpException has occured: " + ex.Message);
      }
      catch (Exception ex)
      {
        throw new AppException("寄信異常" + ex.Message);
      }
    }

    public static async Task mailapiAsync(string to, string subject, string body)
    {
      try
      {
        string content1 = JsonConvert.SerializeObject((object) new SendMailModel()
        {
          sender = new Dictionary<string, string>()
          {
            {
              "email",
              ConfigLib.Get("customer_service_email")
            }
          },
          to = new List<receive>()
          {
            new receive() { email = to }
          },
          subject = subject,
          htmlContent = body
        });
        string requestUri = "https://api.brevo.com/v3/smtp/email";
        StringContent content2 = new StringContent(content1, Encoding.UTF8, "application/json");
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("accept", "application/json");
        httpClient.DefaultRequestHeaders.Add("api-key", "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");
        string str = await (await httpClient.PostAsync(requestUri, (HttpContent) content2)).Content.ReadAsStringAsync();
      }
      catch (Exception ex)
      {
        LogLib.Error(ex);
        throw new AppException(1237, "email_send_exception");
      }
    }
  }
}
