namespace tradeApi2.Models.JYPay;

public class JYDepositCallBak
{
    public string? Mchid { get; set; }
    public string? Out_Trade_No { get; set; }
    public string? Amount { get; set; }
    public string? Transaction_Id { get; set; }
    public string? RefCode { get; set; }
    public string? RefMsg { get; set; }
    public string? Success_Time { get; set; }
    public string? Sign { get; set; }
}