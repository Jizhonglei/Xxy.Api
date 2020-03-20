using Js.DomainDto.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Js.DomainDto.WebUser
{
    public class LoginRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserAccount { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string UserPwd { get; set; }

        /// <summary>
        /// 微信id
        /// </summary>
        public string WechatOpenID { get; set; }

        /// <summary>
        /// 登录方式 1微信openid  2 用户账号密码
        /// </summary>
        public LoginTypeEnum LoginType { get; set; }
    }
}
