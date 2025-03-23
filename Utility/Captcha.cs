// Decompiled with JetBrains decompiler
// Type: StockAdmin.Utility.Captcha
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

// using IronSoftware.Drawing;
using System;
using System.IO;

#nullable enable
namespace StockAdmin.Utility
{
    public static class Captcha
    {
        public static string CreateCode(string dir)
        {
            string[] files = Directory.GetFiles(dir);
            Random random = new Random();
            return Path.GetFileNameWithoutExtension(files[random.Next(files.Length)]);
        }

        public static MemoryStream BuildImage(string captchaCode, string path)
        {
            // using (AnyBitmap anyBitmap = AnyBitmap.FromFile(path).Clone())
            //     return anyBitmap.GetStream();
            return new MemoryStream(File.ReadAllBytes(path));
        }
    }
}