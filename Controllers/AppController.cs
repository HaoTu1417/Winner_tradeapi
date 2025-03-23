// Decompiled with JetBrains decompiler
// Type: tradeapi.Controllers.AppController
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Microsoft.AspNetCore.Mvc;
using Models;
using tradeapi.Common;
using tradeapi.Libs;

#nullable enable
namespace tradeapi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AppController : ApiController
    {
        [HttpPost("appdownloadurl")]
        public APIResponse<DownloadAppResponse> AppDownloadUrl()
        {
            try
            {
                string str = this.Request.Headers.UserAgent.ToString();
                return APIResponse<DownloadAppResponse>.Ok(FileManagementLib.GetDownloadUrl(str.Contains("iPhone") || str.Contains("iPad") ? 0 : 1));
            }
            catch (AppException ex)
            {
                LogLib.Log("[AppController][AppDownloadUrl]" + ex.Message);
                return APIResponse<DownloadAppResponse>.Error(ex.GetStatus(), ex.Message);
            }
        }
    }
}