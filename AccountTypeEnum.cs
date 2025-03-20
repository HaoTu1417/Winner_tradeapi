// Decompiled with JetBrains decompiler
// Type: AccountTypeEnum
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System.ComponentModel.DataAnnotations;

#nullable disable
public enum AccountTypeEnum
{
    [Display(Name = "无")] Unset,
    [Display(Name = "会员")] Member,
    [Display(Name = "管理员")] AdminUser,
    [Display(Name = "运营商")] Agent,
}