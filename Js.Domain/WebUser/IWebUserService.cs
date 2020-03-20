using IFramework.Base;
using Js.DomainDto.Base;
using Js.DomainDto.WebUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Js.Domain
{
    /// <summary>
    /// 用户
    /// </summary>
    public interface IWebUserService : IDependency
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        BaseResponse<LoginResponse> Login(LoginRequest loginRequest);

        /// <summary>
        /// 根据token获取用户信息
        /// </summary>
        /// <param name="tonken"></param>
        /// <returns></returns>
        BaseResponse<WebUserResponse> GetUserInfo(string tonken);


    }
}
