// Decompiled with JetBrains decompiler
// Type: tradeapi.Controllers.ApiController
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using tradeapi.Cache;
using tradeapi.Models;

#nullable enable
namespace tradeapi.Controllers
{
    public class ApiController : ControllerBase
    {
        protected string lang = "VN";

        protected TokenModel GetToken()
        {
            StringValues key;
            return this.Request.Headers.TryGetValue("token", out key) ? TokenCatch.GetToken((string) key) : (TokenModel) null;
        }

        protected string GetIp()
        {
            try
            {
                return this.Request.Headers.Keys.Contains("X-Forwarded-For") ? this.Request.Headers["X-Forwarded-For"].ToString().Split(',')[0] : this.Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}