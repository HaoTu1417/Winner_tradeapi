// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.WithdrawSupportCurrencyService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using Models.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
    public class WithdrawSupportCurrencyService
    {
        public static int FindPkAfterInsert(WithdrawSupportCurrencyDto source)
        {
            string sql = "INSERT INTO `wallet_record` (`code`, `currency`, `type`, `enable`)\n                VALUES (@code, @type, @currency, @type, @enable);\n\n                select @@IDENTITY;";
            try
            {
                using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
                    return writeConntion.ExecuteScalar<int>(sql, (object) source);
            }
            catch (Exception ex)
            {
                LogLib.Error("[WithdrawSupportCurrencyService][FindPkAfterInsert]" + ex.Message);
                throw new AppException(1030, "write_db_exception");
            }
        }

        public static List<string> FindSupportCurrency(int type)
        {
            string sql = "SELECT code From withdraw_support_currency where enable = 1 and type = @type";
            try
            {
                using (IDbConnection readConnection = DapperMysql.GetReadConnection())
                {
                    DynamicParameters parameters = DapperMysql.GetParameters((object) new
                    {
                        type = type
                    });
                    return readConnection.Query<string>(sql, (object) parameters).AsList<string>();
                }
            }
            catch (Exception ex)
            {
                LogLib.Error("[WithdrawSupportCurrencyService][FindSupportCurrency]" + ex.Message);
                throw new AppException(1040, "read_db_exception");
            }
        }
    }
}