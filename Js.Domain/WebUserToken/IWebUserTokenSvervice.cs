using IFramework.Base;
using Js.DomainDto.WebUserToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Js.Domain
{
    public interface IWebUserTokenSvervice: IDependency
    {
        /// <summary>
        /// 获取登录凭证
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        WebUserTokenResponse GetWebUserToken(string userGuid);

        /// <summary>
        /// 获取登录凭证
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        WebUserTokenResponse GetWebUserTokenByToken(string token);

        /// <summary>
        /// 创建登录凭据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        WebUserTokenResponse CreateToken(WebUserTokenRequest request);

        /// <summary>
        /// 更新登录凭据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool UpdateToken(WebUserTokenRequest request);
    }
}
