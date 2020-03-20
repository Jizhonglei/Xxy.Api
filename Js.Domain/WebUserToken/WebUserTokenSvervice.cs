using IFramework.AutoMapper;
using Js.DomainDto.Enum;
using Js.DomainDto.WebUserToken;
using Js.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Js.Domain
{
    public class WebUserTokenSvervice : BaseService, IWebUserTokenSvervice
    {
        public WebUserTokenSvervice(IDbSession dbSession) : base(dbSession)
        {
        }
        /// <summary>
        /// 创建登录凭据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public WebUserTokenResponse CreateToken(WebUserTokenRequest request)
        {
            var result = DbSession.WebUserTokenRepository.Add(request.MapTo<WebUserToken>());
            if (result)
            {
                return GetWebUserToken(request.web_userid);
            }
            return null;
        }

        /// <summary>
        /// 获取登录凭据
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public WebUserTokenResponse GetWebUserToken(string userGuid)
        {
            var tonkenInfo = DbSession.WebUserTokenRepository.Get(u => u.web_userid == userGuid);
            return tonkenInfo.MapTo<WebUserTokenResponse>();
        }
        /// <summary>
        /// 获取登录凭据
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public WebUserTokenResponse GetWebUserTokenByToken(string token)
        {
            var tonkenInfo = DbSession.WebUserTokenRepository.Get(u => u.web_usertoken == token);
            return tonkenInfo.MapTo<WebUserTokenResponse>();
        }

        /// <summary>
        /// 更新登录凭据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool UpdateToken(WebUserTokenRequest request)
        {
            var result = DbSession.WebUserTokenRepository.Update(request.MapTo<WebUserToken>());
            return result;
        }
    }
}
