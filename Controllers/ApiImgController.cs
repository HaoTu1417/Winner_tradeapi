// Decompiled with JetBrains decompiler
// Type: tradeapi.Controllers.ApiImgController
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Validates;
using tradeapi2.Common;

#nullable enable
namespace tradeapi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiImgController : ApiController
    {
        [HttpPost("uploadimages")]
        public async Task<APIResponse> UploadImages([FromForm] UploadImagesRequest input)
        {
            try
            {
                new UploadImagesValidator().ValidateAndThrow<UploadImagesRequest>(input);
                List<int> intList = await UploadImageLib.UploadImages(FileManagementLib.Folder.flag, "table_name", 1, false, input.images);
                return APIResponse.Ok((object) 200, "OK");
            }
            catch (AppException ex)
            {
                return APIResponse.Error(ex.GetStatus(), ex.Message);
            }
            catch (Exception ex)
            {
                LogLib.Warn("[ApiImgController][UploadVerifyIdentityData]" + ex.Message);
                return APIResponse.Error(900, "[900]其他错误");
            }
        }
    }
}