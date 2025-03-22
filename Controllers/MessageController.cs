// Decompiled with JetBrains decompiler
// Type: tradeapi.Controllers.MessageController
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using tradeapi.Business;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models;
using tradeapi.Models.Message;
using tradeapi2.Common;

#nullable enable
namespace tradeapi.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class MessageController : ApiController
  {
    [HttpPost("list")]
    public APIResponse<GetListResponse> GetMessageList(LangRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<GetListResponse>.Ok(MessageBiz.GetMessageList(token.member_fk, 2));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MessageController][GetMessageList]" + ex.Message);
        return APIResponse<GetListResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("listpublic")]
    public APIResponse<GetListPublicResponse> GetListPublic(LangRequest req)
    {
      TokenModel token = this.GetToken();
      try
      {
        return APIResponse<GetListPublicResponse>.Ok(MessageBiz.GetListPublic(token.member_fk));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MessageController][GetListPublic]" + ex.Message);
        return APIResponse<GetListPublicResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("getinternalmessage")]
    public APIResponse<InternalMessageResponse> GetInternalMessage(GetInternalMessageRequest req)
    {
      this.GetToken();
      try
      {
        return APIResponse<InternalMessageResponse>.Ok(MessageBiz.GetInternalMessage(req.pk));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MessageController][GetInternalMessage]" + ex.Message);
        return APIResponse<InternalMessageResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("getpublic")]
    public APIResponse<GetPublicResponse> GetPublicMessage(GetInternalMessageRequest req)
    {
      this.GetToken();
      try
      {
        return APIResponse<GetPublicResponse>.Ok(MessageBiz.GetPublicMessage(req.pk));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MessageController][GetPublicMessage]" + ex.Message);
        return APIResponse<GetPublicResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("send")]
    public async Task<APIResponse> SendMessage(SendRequest req)
    {
      MessageController messageController = this;
      TokenModel token = messageController.GetToken();
      try
      {
        if (req.title == null || "".Equals(req.title))
          return APIResponse.Error(2002, TranslatorService.ConvertByKey(messageController.lang, "error_title_required"));
        if (req.content == null || "".Equals(req.content))
          return APIResponse.Error(2003, TranslatorService.ConvertByKey(messageController.lang, "error_content_required"));
        int num = await MessageBiz.SendMessage(req, token.member_fk) ? 1 : 0;
        return APIResponse.Ok((object) null);
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MessageController][SendMessage]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("deleteinternal")]
    public APIResponse DeleteInternalMessages(DeleteInternalRequest req)
    {
      this.GetToken();
      try
      {
        MessageBiz.DeleteInternalMessages(req.PkList);
        return APIResponse.Ok((object) null);
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MessageController][DeleteInternalMessages]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("read")]
    public APIResponse Read(ReadRequest req)
    {
      try
      {
        MessageBiz.Read(req.pk_list);
        return APIResponse.Ok((object) null);
      }
      catch (AppException ex)
      {
        LogLib.Warn("[MessageController][Read]" + ex.Message);
        return APIResponse.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }
  }
}
