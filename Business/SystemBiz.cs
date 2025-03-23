// Decompiled with JetBrains decompiler
// Type: tradeapi.Business.SystemBiz
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using DB.Services;
using Models.Dto;
using System.Collections.Generic;
using tradeapi.Models.Dto;

#nullable enable
namespace tradeapi.Business
{
    public class SystemBiz
    {
        public static List<MutilangSubjectDto> GetLangList() => MutilangSubjectService.FindAll();

        public static void SetLang(string lang)
        {
        }

        public static List<SysCountryDto> CountryList(string lang) => SysCountryService.FindAll(lang);
    }
}