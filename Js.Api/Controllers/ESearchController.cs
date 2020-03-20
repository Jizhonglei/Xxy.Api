using Js.Api.App_Start.Skip;
using Js.Domain;
using Js.DomainDto.Base;
using Js.DomainDto.Response;
using Js.DomainDto.WebUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace Js.Api.Controllers
{
    /// <summary>
    /// ES检索接口
    /// </summary>
    /// <remarks>2020.3.13 纪钟磊 创建</remarks>
    public class ESearchController : BaseController
    {
        private readonly IESearchService _eSearchService;

        /// <summary>
        /// ESearchController构造函数注入
        /// </summary>
        /// <param name="eSearchService"></param>
        public ESearchController(IESearchService eSearchService)
        {
            _eSearchService = eSearchService;
        }

        /// <summary>
        /// 全局索引接口
        /// </summary>
        /// <param name="request">查询条件</param>
        /// <returns>对应json数据</returns>
        /// <remarks>2020.3.14 纪钟磊 创建</remarks>
        [ApiLogin]
        [HttpPost]
        public BaseResponse<ImproveSearchResponse> ImproveSearch(ImproveSearchRequst request)
        {
            return _eSearchService.GetImproveSearch(request);
        }

        /// <summary>
        /// 索引接口按条件
        /// </summary>
        /// <param name="type">查询类型</param>
        /// <param name="guid">guid</param>
        /// <returns>对应json数据</returns>
        /// <remarks>2020.3.14 纪钟磊 创建</remarks>
        [HttpGet]
        public BaseResponse<ImproveConditionResponse> GetImproveByConditon(string type, string guid)
        {
            return _eSearchService.GetImproveByConditon(type,guid);
        }
    }
}

