using IFramework.Base;
using Js.Api.App_Start.Skip;
using Js.Domain;
using Js.DomainDto.Base;
using Js.DomainDto.WebUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Js.Api.Controllers
{
    /// <summary>
    /// 用户相关操作
    /// </summary>
    public class WebUserController : BaseController
    {
        private readonly IWebUserService _webUserService;
        /// <summary>
        /// 注入接口
        /// </summary>
        /// <param name="webUserService"></param>
        public WebUserController(IWebUserService webUserService)
        {
            _webUserService = webUserService;
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ApiLogin]
        public BaseResponse<LoginResponse> Login(BaseRequest<LoginRequest> request)
        {

            return _webUserService.Login(request.Request);
        }
 
    }
}
