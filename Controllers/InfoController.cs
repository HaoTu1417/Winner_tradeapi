// Decompiled with JetBrains decompiler
// Type: tradeapi.Controllers.InfoController
// Assembly: tradeapi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B1DD9E6-779B-413A-AAC1-D3429DA62127
// Assembly location: /Users/tunghaotu/www/service/tradeapi/tradeapi.dll

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using tradeapi.Business;
using tradeapi.Common;
using tradeapi.Libs;
using tradeapi.Models;
using tradeapi.Models.Info;

#nullable enable
namespace tradeapi.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class InfoController : ApiController
  {
    [HttpPost("promotionlist")]
    public APIResponse<List<PromotionListResponse>> PromotionList(LangRequest req)
    {
      try
      {
        return APIResponse<List<PromotionListResponse>>.Ok(InfoBiz.GetPromotionList(req.lang));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[InfoController][PromotionList]" + ex.Message);
        return APIResponse<List<PromotionListResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("promotioncontent")]
    public APIResponse<PromotionContentResponse> PromotionContent(PromotionContentRequest req)
    {
      try
      {
        return APIResponse<PromotionContentResponse>.Ok(InfoBiz.GetPromotionContent(req.pk));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[InfoController][PromotionContent]" + ex.Message);
        return APIResponse<PromotionContentResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("questioncatalog")]
    public APIResponse<List<QuestionCatalogResponse>> QuestionCatalog(LangRequest req)
    {
      try
      {
        return APIResponse<List<QuestionCatalogResponse>>.Ok(InfoBiz.GetQuestionCatalog(req.lang));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[InfoController][QuestionCatalog]" + ex.Message);
        return APIResponse<List<QuestionCatalogResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("questionlist")]
    public APIResponse<List<QuestionListResponse>> QuestionList(QuestionListRequest req)
    {
      try
      {
        return APIResponse<List<QuestionListResponse>>.Ok(InfoBiz.GetQuestionList(req.lang, req.catalog_pk));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[InfoController][QuestionList]" + ex.Message);
        return APIResponse<List<QuestionListResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("answer")]
    public APIResponse<AnswerResponse> Answer(AnswerRequest req)
    {
      try
      {
        return APIResponse<AnswerResponse>.Ok(InfoBiz.GetAnswer(req.lang, req.question_pk));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[InfoController][Answer]" + ex.Message);
        return APIResponse<AnswerResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("exchangerate")]
    public APIResponse<ExchangeRateResponse> ExchangeRate(ExchangeRateRequest req)
    {
      this.GetToken();
      try
      {
        return APIResponse<ExchangeRateResponse>.Ok(InfoBiz.ExchangeRate(req.target_currency, req.base_currency));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[InfoController][ExchangeRate]" + ex.Message);
        return APIResponse<ExchangeRateResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("exchange")]
    public APIResponse<List<List<ExchangeResponse>>> Exchange(LangRequest req)
    {
      try
      {
        return APIResponse<List<List<ExchangeResponse>>>.Ok(InfoBiz.Exchange());
      }
      catch (AppException ex)
      {
        LogLib.Warn("[InfoController][Exchange]" + ex.Message);
        return APIResponse<List<List<ExchangeResponse>>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("bulletin")]
    public APIResponse<List<BulletinResponse>> Bulletin(LangRequest req)
    {
      try
      {
        return APIResponse<List<BulletinResponse>>.Ok(InfoBiz.Bulletin(req.lang));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[InfoController][Bulletin]" + ex.Message);
        return APIResponse<List<BulletinResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("bulletincontent")]
    public APIResponse<BulletinContentResponse> BulletinContent(BulletinContentResquest req)
    {
      try
      {
        return APIResponse<BulletinContentResponse>.Ok(InfoBiz.BulletinContent(req.pk));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[InfoController][BulletinContent]" + ex.Message);
        return APIResponse<BulletinContentResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("doc")]
    public APIResponse<DocResponse> Doc(DocRequest req)
    {
      try
      {
        return APIResponse<DocResponse>.Ok(InfoBiz.Doc(req));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[InfoController][Doc]" + ex.Message);
        return APIResponse<DocResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("docbycid")]
    public APIResponse<DocResponse> DocByCID(DocByCIDRequest req)
    {
      try
      {
        return APIResponse<DocResponse>.Ok(InfoBiz.GetDocByCid(req.cid, req.lang));
      }
      catch (AppException ex)
      {
        LogLib.Error((Exception) ex);
        return APIResponse<DocResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("service")]
    public APIResponse<ServiceResponse> Service(LangRequest req)
    {
      try
      {
        return APIResponse<ServiceResponse>.Ok(InfoBiz.Service(req.lang));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[InfoController][Service]" + ex.Message);
        return APIResponse<ServiceResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("servicequestion")]
    public APIResponse<List<ServiceQuestioResponse>> ServiceQuestion(QuestionListRequest req)
    {
      try
      {
        return APIResponse<List<ServiceQuestioResponse>>.Ok(InfoBiz.GetServiceQuestion(req.lang, req.catalog_pk));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[InfoController][ServiceQuestion]" + ex.Message);
        return APIResponse<List<ServiceQuestioResponse>>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("serviceanswer")]
    public APIResponse<ServiceAnswerResponse> serviceanswer(ServiceAnswerRequest req)
    {
      try
      {
        return APIResponse<ServiceAnswerResponse>.Ok(InfoBiz.GetServiceAnswer(req.lang, req.question_pk));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[InfoController][serviceanswer]" + ex.Message);
        return APIResponse<ServiceAnswerResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("ad")]
    public APIResponse<AdResponse> Ad(LangRequest req)
    {
      try
      {
        return APIResponse<AdResponse>.Ok(InfoBiz.GetAd(req.lang));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[InfoController][Ad]" + ex.Message);
        return APIResponse<AdResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }

    [HttpPost("advertise")]
    public APIResponse<AdvertiseResponse> Advertise(LangRequest req)
    {
      try
      {
        return APIResponse<AdvertiseResponse>.Ok(InfoBiz.GetAdvertise(req.lang));
      }
      catch (AppException ex)
      {
        LogLib.Warn("[InfoController][Advertise]" + ex.Message);
        return APIResponse<AdvertiseResponse>.Error(ex.GetStatus(), ex.GetMessage(req.lang));
      }
    }
  }
}
