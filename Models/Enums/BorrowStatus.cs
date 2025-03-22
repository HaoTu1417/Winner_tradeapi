// Decompiled with JetBrains decompiler
// Type: BorrowStatus
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System.ComponentModel.DataAnnotations;

#nullable disable
public enum BorrowStatus
{
    [Display(Name = "待审核")] Default = -1, // 0xFFFFFFFF
    [Display(Name = "未通过")] Forbid = 0,
    [Display(Name = "使用中")] Using = 1,
    [Display(Name = "已结束")] End = 2,
    [Display(Name = "已逾期")] Expired = 3,
}