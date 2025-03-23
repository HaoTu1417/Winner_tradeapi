// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.RichboxConfigService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using System;
using System.Data;
using System.Linq;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Dto;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
    public class RichboxConfigService
    {
        public static RichboxConfigDto? Find()
        {
            string sql = "\nSELECT `id`, `enable`, `active_date`, `diactive_date`, `currency`, `min_investment`, `max_investment`, `interest_rate`, `begin_profit`, `closing_time`, `give_interest_time`, `feature`, `description`, `trade_info`\nFROM `richbox_config`\nWHERE 1\n";
            try
            {
                using (IDbConnection readConnection = DapperMysql.GetReadConnection())
                    return readConnection != null ? readConnection.Query<RichboxConfigDto>(sql).AsList<RichboxConfigDto>().LastOrDefault<RichboxConfigDto>() : (RichboxConfigDto) null;
            }
            catch (Exception ex)
            {
                LogLib.Error("[RichboxConfigService][Find]" + ex.Message);
                throw new AppException(1040, "read_db_exception");
            }
        }
    }
}