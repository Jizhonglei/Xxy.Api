using IFramework.AutoMapper;
using IFramework.Base;
using IFramework.Encryption;
using Js.DomainDto.Base;
using Js.DomainDto.Enum;
using Js.DomainDto.WebUser;
using Js.DomainDto.WebUserToken;
using Js.Entity;
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
    public class WebUserService : BaseService, IWebUserService
    {
        private readonly IWebUserTokenSvervice _webUserTokenSvervice;
        public WebUserService(IDbSession dbSession, IWebUserTokenSvervice webUserTokenSvervice) : base(dbSession)
        {
            _webUserTokenSvervice = webUserTokenSvervice;
        }
        /// <summary>
        /// 根据登录凭证获取用户信息
        /// </summary>
        /// <param name="tonken"></param>
        /// <returns></returns>
        public BaseResponse<WebUserResponse> GetUserInfo(string tonken)
        {
            var tokenInfo = _webUserTokenSvervice.GetWebUserTokenByToken(tonken);
            if (tokenInfo == null)
                AppException(AppErrorEnum.Toekn_NotFind);
            var userInfo = DbSession.WebUserRepository.Get(u => u.user_id == tokenInfo.web_userid);
            return new BaseResponse<WebUserResponse>() { Data = userInfo.MapTo<WebUserResponse>(), IsSucceed = true };
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        public BaseResponse<LoginResponse> Login(LoginRequest loginRequest)
        {
            //通过微信id登录
            if (loginRequest.LoginType == LoginTypeEnum.WechatOpenID)
            {
                var result = DbSession.WebUserRepository.Get(u => u.user_wid == loginRequest.WechatOpenID);
                //准备一个登录凭证
                WebUserTokenRequest tokenAdd = new WebUserTokenRequest();
                tokenAdd.guids = Guid.NewGuid().ToString();
                tokenAdd.login_date = DateTime.Now;
                tokenAdd.expirydate = tokenAdd.login_date.AddDays(10);
                if (result != null)
                {
                    //找到登录凭证需要创建登录凭证
                    var tokeninfo = _webUserTokenSvervice.GetWebUserToken(result.user_id);
                    //未找到登录凭证
                    if (tokeninfo == null)
                    {
                        tokenAdd.web_userid = result.user_id;
                        tokenAdd.web_usertoken = EncryptionFactory.Md5Encrypt(result.user_id);
                        var tokenInfo = _webUserTokenSvervice.CreateToken(tokenAdd);
                        return Response(new LoginResponse() { Token = tokenInfo.web_usertoken },true);
                    }
                    else
                    {
                        tokenAdd.web_userid = result.user_id;
                        tokenAdd.web_usertoken = EncryptionFactory.Md5Encrypt(result.user_id);
                        DbSession.Transaction(dt =>
                        {
                            DbSession.WebUserTokenRepository.Delete(u => u.web_userid == result.user_id,false, transaction: dt);
                            
                            DbSession.WebUserTokenRepository.Add(tokenAdd.MapTo<WebUserToken>(), transaction: dt);
                        });
                        return Response(new LoginResponse() { Token = tokenAdd.web_usertoken },true);
                    }
                }
                else {
                    WebUserRequest webUserAdd = new WebUserRequest();
                    webUserAdd.user_id = Guid.NewGuid().ToString();
                    webUserAdd.user_wid = loginRequest.WechatOpenID;
                    webUserAdd.user_regdate = DateTime.Now;
                    webUserAdd.user_state = 0;
                    //创建一个登录账号

                    var webUserAddResult = DbSession.WebUserRepository.Add(webUserAdd.MapTo<WebUser>());
                    if (!webUserAddResult)
                        AppException(AppErrorEnum.LoginError);
                    //创建登录凭证
                    tokenAdd.web_userid = webUserAdd.user_id;
                    tokenAdd.web_usertoken = EncryptionFactory.Md5Encrypt(webUserAdd.user_id);
                    var tokenInfo = _webUserTokenSvervice.CreateToken(tokenAdd);
                    return Response(new LoginResponse() { Token = tokenAdd.web_usertoken },true);
                }

            }
            //通过账号密码登录
            else if (loginRequest.LoginType == LoginTypeEnum.Account)
            {


            }
            return null;
        }

    }
}
