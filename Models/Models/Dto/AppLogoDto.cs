// Decompiled with JetBrains decompiler
// Type: Models.Dto.AppLogoDto
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

#nullable enable
namespace Models.Dto
{
    public class AppLogoDto
    {
        public int cms_files_fk { get; set; }

        public int type { get; set; }

        public string type_string
        {
            get
            {
                switch (this.type)
                {
                    case 1:
                        return "登入頁面logo";
                    case 2:
                        return "下載頁面logo";
                    case 3:
                        return "下載頁面背景圖";
                    case 4:
                        return "通用logo";
                    case 5:
                        return "推廣底圖";
                    default:
                        return "";
                }
            }
        }

        public string enable { get; set; }

        public string lang { get; set; }

        public int sort { get; set; }

        public string url { get; set; }
    }
}