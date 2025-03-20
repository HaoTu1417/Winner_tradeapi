// Decompiled with JetBrains decompiler
// Type: MessageTransTypeEnum
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System.ComponentModel.DataAnnotations;

#nullable disable
public enum MessageTransTypeEnum
{
    [Display(Name = "Internal")] Internal = 1,
    [Display(Name = "email")] Email = 2,
    [Display(Name = "SMS")] Sms = 3,
}