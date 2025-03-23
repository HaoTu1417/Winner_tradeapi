// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.ViewTradeAccountService
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Dapper;
using System;
using System.Data;
using System.Runtime.CompilerServices;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models.Dto;
using tradeapi.Models.SubAccount;
using tradeapi.Utility;

#nullable enable
namespace tradeapi.Services
{
  public class ViewTradeAccountService
  {
    public static System.Collections.Generic.List<TradeAccountResponse> GetValidTradeAccounts(
      int member_id)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(634, 1);
      interpolatedStringHandler.AppendLiteral("SELECT vw_trade_account.*, (vw_trade_account.margin + vw_trade_account.loan_money) AS init_money, \n                IF (vw_trade_account.STATUS <= 1, 0, 1) AS is_over, \n                borrow.borrow_interest/borrow_duration AS management_fee, \n                (vw_trade_account.balance - vw_trade_account.position_value - vw_trade_account.frozen_money) AS available_balance, \n                vw_trade_account.multiple \n                   FROM `vw_trade_account`\n                 LEFT JOIN borrow ON vw_trade_account.sub_account = borrow.sub_account\n                   WHERE vw_trade_account.member_fk = ");
      interpolatedStringHandler.AppendFormatted<int>(member_id);
      interpolatedStringHandler.AppendLiteral(" AND vw_trade_account.status <=1");
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<TradeAccountResponse>(stringAndClear).AsList<TradeAccountResponse>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[ViewTradeAccountService][GetValidTradeAccounts]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static System.Collections.Generic.List<TradeAccountResponse> List(int member_id)
    {
      string sql = "SELECT *, (vw_trade_account.margin + vw_trade_account.loan_money) AS init_money, IF (vw_trade_account.STATUS <= 1, 0, 1) AS is_over, borrow_fee.borrow_fee AS management_fee, (vw_trade_balance.balance - vw_trade_balance.position_value - vw_trade_account.frozen_money) AS available_balance, trade_account.multiple AS multiple \n                           FROM `vw_trade_account`\n                           LEFT JOIN borrow_fee ON vw_trade_account.sub_account = borrow_fee.sub_account\n                           LEFT JOIN vw_trade_balance ON vw_trade_account.sub_account = vw_trade_balance.sub_account\n                           LEFT JOIN trade_account ON vw_trade_account.sub_account = trade_account.sub_account\n                           WHERE vw_trade_account.member_fk = @member_id";
      var data = new{ member_id = member_id };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<TradeAccountResponse>(sql, (object) data).AsList<TradeAccountResponse>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[ViewTradeAccountService][List]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static ViewTradeAccountDto GetViewTradeAccount(string sub_account)
    {
      string sql = "SELECT * FROM `vw_trade_account` WHERE sub_account = @sub_account";
      var data = new{ sub_account = sub_account };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QuerySingleOrDefault<ViewTradeAccountDto>(sql, (object) data);
      }
      catch (Exception ex)
      {
        LogLib.Error("[ViewTradeAccountService][GetViewTradeAccount]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }
  }
}
