using IFramework.Base;
using Js.DomainDto.Base;
using Js.IFramework.Utility.Helper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace Js.Api.App_Start.Filters
{
    /// <summary>
    /// webapi异常接管
    /// </summary>
    public class WebApiErrorAttribute: ExceptionFilterAttribute
    {
        /// <summary>
        /// 重写异常处理
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            string errMessage = actionExecutedContext.Exception.Message;
            if (string.IsNullOrEmpty(errMessage))
            {
                errMessage = actionExecutedContext.Exception.Message;
            }
            BaseResponse<string> rsp = new BaseResponse<string>() {
                IsSucceed = false,
                Code = -1,
                Err = errMessage,
            };
        
            JObject rspjObject = rsp.ObjectToJSON().Replace("\":null", "\":\"\"").JSONToObject<JObject>();

            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(System.Net.HttpStatusCode.OK, rspjObject);
            //  actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            base.OnException(actionExecutedContext);
        }
    }
}