// Decompiled with JetBrains decompiler
// Type: tradeapi.Controllers.SystemController
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Microsoft.AspNetCore.Mvc;
using Models.Dto;
using System.Collections.Generic;
using tradeapi.Business;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models;
using tradeapi.Models.Dto;
using tradeapi.Models.System;
using tradeapi2.Common;

#nullable enable
namespace tradeapi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SystemController : ApiController
    {
        [HttpPost("getlanglist")]
        public APIResponse<List<MutilangSubjectDto>> GetLangList(GetLangListRequest req)
        {
            try
            {
                return APIResponse<List<MutilangSubjectDto>>.Ok(SystemBiz.GetLangList());
            }
            catch (AppException ex)
            {
                LogLib.Warn("[SystemController][GetLangList]" + ex.Message);
                return APIResponse<List<MutilangSubjectDto>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
            }
        }

        [HttpPost("setlang")]
        public APIResponse SetLang(SetLangRequest req)
        {
            try
            {
                SystemBiz.SetLang(req.lang);
                return APIResponse.Ok((object) "");
            }
            catch (AppException ex)
            {
                LogLib.Warn("[SystemController][SetLang]" + ex.Message);
                return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
            }
        }

        [HttpPost("countrylist")]
        public APIResponse<List<SysCountryDto>> CountryList(LangRequest req)
        {
            this.GetToken();
            try
            {
                return APIResponse<List<SysCountryDto>>.Ok(SystemBiz.CountryList(req.lang));
            }
            catch (AppException ex)
            {
                LogLib.Warn("[SystemController][CountryList]" + ex.Message);
                return APIResponse<List<SysCountryDto>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
            }
        }
    }
}