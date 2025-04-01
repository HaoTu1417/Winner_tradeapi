namespace tradeApi2.Models.JYPay;

public class JYPayAddRequest
{
    // mã số khách hàng do bên JY cấp, mặt định là 6126
    private string _mchid = "6126";
    public string mchid { get=>_mchid; set=>_mchid=value; }
    // mã số đơn hàng bên JY, phải là unique
    public string out_trade_no{ get; set; }
    // số tiền, chừa 2 số sau dấy phẩy 50000,00
    public float money{ get; set; }
    // notifyurl 
    public string notifyurl{ get; set; }
    // loại hình thanh toán, ngân hàng, thẻ momo
    public string code{ get; set; }
    // thời gian yêu cầu theo format 2001-01-01 18:00:00
    public string applydate { get; set; }

    // ip cùa người chuyueenr tiền, trường này bắt buộc nhưng không bao gồm trong sign
    private string _client_ip = "";

    public string client_ip
    {
        get => _client_ip;  
        set => _client_ip = value?.Trim();
    }
    
    // optional, url redirect, sau khi thanh toán thành dcoong hoặc thất bại sẽ redirect theo url này
    private string _returnurl = "";
    public string returnurl
    {
        get => _returnurl;
        set => _returnurl = value?.Trim();
    }

    // option tên sản phẩm, không bao gồm trong sign
    private string _productname = "";
    public string productname
    {
        get => _productname;
        set => _productname = value?.Trim();
    }
    // optional, thông tin note thêm, sẽ return y như vậy.không bao gồm trong sign
    private string _attach = "";
    public string attach
    {
        get => _attach;
        set => _attach = value?.Trim();
    }
    
    // optional, tên người chuyển khoản,
    // khi chọn hình thức thanh toán qua thẻ bắt buộc điền. không bao gồm trong sign
    private string _submitName = "";
    public string submitname
    {
        get => _submitName;
        set => _submitName = value?.Trim();
    }
    // sign để check sum cho dữ liệu truyền vào
    
    public string sign{get;set;}
}