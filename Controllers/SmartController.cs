// Decompiled with JetBrains decompiler
// Type: tradeapi.Controllers.SmartController
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using tradeapi.Business;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Smart;

#nullable enable
namespace tradeapi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SmartController : ApiController
    {
        [HttpPost("getranking")]
        public APIResponse<List<GetRankingResponse>> GetRanking(GetRankingRequest req)
        {
            this.GetToken();
            try
            {
                return APIResponse<List<GetRankingResponse>>.Ok(SmartBiz.GetRanking());
            }
            catch (AppException ex)
            {
                LogLib.Warn("[SmartController][GetRanking]" + ex.Message);
                return APIResponse<List<GetRankingResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
            }
        }
    }
}