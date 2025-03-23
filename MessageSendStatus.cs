// Decompiled with JetBrains decompiler
// Type: MessageSendStatus
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using System.ComponentModel.DataAnnotations;

#nullable disable
public enum MessageSendStatus
{
    [Display(Name = "Draft")] Draft,
    [Display(Name = "Sented")] Sented,
    [Display(Name = "Failed")] Failed,
}