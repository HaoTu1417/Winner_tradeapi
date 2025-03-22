// Decompiled with JetBrains decompiler
// Type: tradeapi.Validates.UploadImagesValidator
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using FluentValidation;
using Microsoft.AspNetCore.Http;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

#nullable enable
namespace tradeapi.Validates
{
    public class UploadImagesValidator : AbstractValidator<UploadImagesRequest>
    {
        public UploadImagesValidator()
        {
            // [2130] Image list must not be null or empty
            RuleFor(x => x.images)
                .Must(list => list != null && list.Count > 0)
                .WithMessage("[2130] 图片数量错误");

            // [2120] Each image must be of valid format
            RuleForEach(x => x.images)
                .Must(IsValidImageFormat)
                .WithMessage("[2120] 图片格式错误");

            // [2121] Image folder is required
            RuleFor(x => x.img_folder)
                .NotEmpty()
                .WithMessage("[2121] 圖片資料夾為必填");

            // [2122] Image folder must be one of the allowed categories
            RuleFor(x => x.img_folder)
                .Must(folder => new[] { "flag", "id", "banner", "article", "message", "bank_book" }
                    .Contains(folder))
                .WithMessage("[2122] 不存在的圖片分類");

            // [2123] Table name is required
            RuleFor(x => x.table_name)
                .NotEmpty()
                .WithMessage("[2123] 圖片對應表名稱為必填");

            // [2124] pk must not be zero
            RuleFor(x => x.pk)
                .Must(pk => pk != 0)
                .WithMessage("[2124] 表的pk為必填");
        }

        private bool IsValidImageFormat(IFormFile file)
        {
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/jpg", "image/gif" };
            return file != null && allowedTypes.Contains(file.ContentType);
        }
    }
}
