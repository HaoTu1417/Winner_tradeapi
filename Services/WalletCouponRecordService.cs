// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.WalletCouponRecordService
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
  public class WalletCouponRecordService
  {
    public static WalletCouponRecordDto Find(int pk)
    {
      string sql = "SELECT * FROM `wallet_coupon_record` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return readConnection.QueryFirstOrDefault<WalletCouponRecordDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletCouponRecordService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static WalletCouponRecordDto Find(int pk, int member_fk)
    {
      string sql = "SELECT * FROM `wallet_coupon_record` WHERE `pk` = @pk AND `member_fk` = @member_fk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk,
            member_fk = member_fk
          });
          return readConnection.QueryFirstOrDefault<WalletCouponRecordDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletCouponRecordService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<WalletCouponRecordDto> FindAll()
    {
      string sql = "SELECT * FROM `wallet_coupon_record`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<WalletCouponRecordDto>(sql).AsList<WalletCouponRecordDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletCouponRecordService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<WalletCouponRecordDto> FindAllByMemberFK(int member_fk)
    {
      string sql = "SELECT * FROM `wallet_coupon_record` WHERE `member_fk` = @member_fk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<WalletCouponRecordDto>(sql, (object) new
          {
            member_fk = member_fk
          }).AsList<WalletCouponRecordDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletCouponRecordService][FindAllByMemberFK]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int FindPkAfterInsert(WalletCouponRecordDto source)
    {
      string sql = "INSERT INTO `wallet_coupon_record` (\n                `member_fk`, `cms_promotion_fk`, `currency`, `affect`, `exchange`, `wallet_amount` , `coupon_balance`, `money_type`, `type`, `sub_type`, `info`, `create_time`, `create_user`, `sended`, `send_time`, `param`)\n                VALUES (@member_fk, @cms_promotion_fk, @currency, @affect, @exchange, @wallet_amount, @coupon_balance, @money_type, @type, @sub_type, @info, @create_time, @create_user, @sended, @send_time, @param);\n\n                select @@IDENTITY;";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.ExecuteScalar<int>(sql, (object) source);
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletCouponRecordService][FindPkAfterInsert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateFull(WalletCouponRecordDto model)
    {
      string sql = "UPDATE `wallet_coupon_record` SET \n                `member_fk` = @member_fk,\n                `cms_promotion_fk` = @cms_promotion_fk,\n                `currency` = @currency,\n                `affect` = @affect,\n                `exchange` = @exchange,\n                `wallet_amount` = @wallet_amount,\n                `coupon_balance` = @coupon_balance,\n                `money_type` = @money_type,\n                `type` = @type,\n                `sub_type` = @sub_type,\n                `info` = @info,\n                `create_time` = @create_time,\n                `create_user` = @create_user,\n                `sended` = @sended,\n                `send_time` = @send_time\n                 WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletCouponRecordService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateStatusAndSendTimeAndSendUser(
      int cms_promotion_fk,
      DateTime send_time,
      string give_out_user)
    {
      string sql = "UPDATE `wallet_coupon_record` SET \n                `sended` = 1,\n                `send_time` = @send_time,\n                `create_user` = @give_out_user\n                 WHERE `cms_promotion_fk` = @cms_promotion_fk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            cms_promotion_fk = cms_promotion_fk,
            send_time = send_time,
            give_out_user = give_out_user
          });
          return writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletCouponRecordService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateWalletCoupon(WalletCouponRecordDto model)
    {
      string sql = "UPDATE `wallet_coupon_record` SET \n                `currency` = @currency,\n                `affect` = @affect,\n                `exchange` = @exchange,\n                `wallet_amount` = @wallet_amount,\n                `money_type` = @money_type,\n                `info` = @info\n                 WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletCouponRecordService][UpdateWalletCoupon]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Remove(int pk)
    {
      string sql = "DELETE FROM `wallet_coupon_record` WHERE `pk` = @pk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            pk = pk
          });
          return writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletCouponRecordService][Remove]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static List<WalletCouponRecordDto> FindAll(string whereSql = "")
    {
      string sql = "SELECT * FROM `wallet_coupon_record` " + whereSql;
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<WalletCouponRecordDto>(sql).AsList<WalletCouponRecordDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletCouponRecordService][FindWalletCouponRecordList]" + ex.Message);
        return (List<WalletCouponRecordDto>) null;
      }
    }

    public static WalletCouponRecordDto FindByMemberFK_Type_SubType(
      int member_fk,
      int type,
      int sub_type)
    {
      string sql = "SELECT * FROM `wallet_coupon_record` \nWHERE `member_fk` = @member_fk AND `type` = @type AND `sub_type` = @sub_type";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.QueryFirstOrDefault<WalletCouponRecordDto>(sql, (object) new
          {
            member_fk = member_fk,
            type = type,
            sub_type = sub_type
          });
      }
      catch (Exception ex)
      {
        LogLib.Error("[WalletCouponRecordService][FindByMemberFK_Type_SubType]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }
  }
}
