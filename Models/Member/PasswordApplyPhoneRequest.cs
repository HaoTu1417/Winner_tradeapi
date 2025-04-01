using tradeapi.Models;

namespace tradeApi2.Models.Member;

public class PasswordApplyPhoneRequest: LangRequest
{
    private string _phone = "";

    public string phone
    {
        get => this._phone;
        set => this._phone = value.ToLower();
    }

    public string phone_verifyCode { get; set; } = "";

    public string newpasswd { get; set; } = "";

    public string captcha { get; set; } = "";

}