// Decompiled with JetBrains decompiler
// Type: tradeapi.Services.RecommendRegisterService
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
  public class RecommendRegisterService
  {
    public static RecommendRegisterDto Find(int invitee_fk)
    {
      string sql = "SELECT * FROM `recommend_register` WHERE `invitee_fk` = @invitee_fk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            invitee_fk = invitee_fk
          });
          return readConnection.QueryFirstOrDefault<RecommendRegisterDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRegisterService][Find]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static List<RecommendRegisterDto> FindAll()
    {
      string sql = "SELECT * FROM `recommend_register`";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          return readConnection.Query<RecommendRegisterDto>(sql).AsList<RecommendRegisterDto>();
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRegisterService][FindAll]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int Insert(RecommendRegisterDto model)
    {
      string sql = "INSERT INTO `recommend_register` (\n                `member_fk`, `invitee_fk`, `register_date`, `first_borrow_date`)\n                VALUES (@member_fk, @invitee_fk, @register_date, @first_borrow_date); ";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRegisterService][Insert]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int UpdateFull(RecommendRegisterDto model)
    {
      string sql = "UPDATE `recommend_register` SET \n                `register_date` = @register_date,\n                `first_borrow_date` = @first_borrow_date\n                 WHERE `Recommend_fk` = @Recommend_fk AND `invitee_fk` = @invitee_fk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) model);
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRegisterService][UpdateFull]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int Remove(int Recommend_fk, int invitee_fk)
    {
      string sql = "DELETE FROM `recommend_register` WHERE `Recommend_fk` = @Recommend_fk AND `invitee_fk` = @invitee_fk";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            Recommend_fk = Recommend_fk,
            invitee_fk = invitee_fk
          });
          return writeConntion.Execute(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRegisterService][Remove]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static List<RecommendRegisterResponse> FindByMemberFk(int member_fk)
    {
      string sql = "SELECT IFNULL(member.real_name, '') as nickname, recommend_register.register_date, recommend_register.first_borrow_date FROM `recommend_register`\n                           LEFT JOIN member ON recommend_register.invitee_fk = member.pk\n                           WHERE recommend_register.member_fk = @member_fk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return readConnection.Query<RecommendRegisterResponse>(sql, (object) parameters).AsList<RecommendRegisterResponse>();
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRegisterService][FindByMemberFk]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static RecommendRegisterDto FindByInviteeFk(int member_fk)
    {
      string sql = "SELECT * FROM `recommend_register` WHERE `invitee_fk` = @member_fk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return readConnection.QueryFirstOrDefault<RecommendRegisterDto>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRegisterService][FindByInviteeFk]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static void SetRecommendFirstBorrowDate(int pk, DateTime date)
    {
      string sql = "UPDATE recommend_register SET first_borrow_date = @date\n                           WHERE invitee_fk = @pk";
      var data = new{ pk = pk, date = date };
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
          readConnection.Execute(sql, (object) data);
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRegisterService][SetRecommendFirstBorrowDate]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }

    public static int GetAllInvitationsByMemberFk(int member_fk)
    {
      string sql = "\nSELECT COUNT(*) \nFROM `recommend_register` \nWHERE `member_fk` = @member_fk";
      try
      {
        using (IDbConnection readConnection = DapperMysql.GetReadConnection())
        {
          DynamicParameters parameters = DapperMysql.GetParameters((object) new
          {
            member_fk = member_fk
          });
          return readConnection.QuerySingle<int>(sql, (object) parameters);
        }
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRegisterService][GetAllInvitationsByMemberFk]" + ex.Message);
        throw new AppException(1040, "read_db_exception");
      }
    }

    public static int UpdateFirstBorrowDate(int member, DateTime borrowDate)
    {
      string sql = "UPDATE `recommend_register` SET \n                `first_borrow_date` = @first_borrow_date\n                 WHERE `invitee_fk` = @invitee_fk AND `first_borrow_date` IS NULL";
      try
      {
        using (IDbConnection writeConntion = DapperMysql.GetWriteConntion())
          return writeConntion.Execute(sql, (object) new
          {
            invitee_fk = member,
            first_borrow_date = borrowDate
          });
      }
      catch (Exception ex)
      {
        LogLib.Error("[RecommendRegisterService][UpdateFirstBorrowDate]" + ex.Message);
        throw new AppException(1030, "write_db_exception");
      }
    }
  }
}
