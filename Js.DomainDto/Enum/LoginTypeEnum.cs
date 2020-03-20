using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Js.DomainDto.Enum
{
    /// <summary>
    /// 登录方式
    /// </summary>
    public enum LoginTypeEnum
    {
        [Description("微信Openid登录")]
        WechatOpenID=1,

        [Description("账号密码登录")]
        Account=2
    }
}
