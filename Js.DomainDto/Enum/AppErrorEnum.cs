using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Js.DomainDto.Enum
{
    /// <summary>
    /// 系统错误信息
    /// </summary>
    public enum AppErrorEnum
    {
        /// <summary>
        /// 登录失败
        /// </summary>
        [Description("登录失败")]
        LoginError = 10001,

        /// <summary>
        /// 创建用户账号失败
        /// </summary>
        [Description("创建用户账号失败")]
        CreateUserError = 10001,

        /// <summary>
        /// 未找到token
        /// </summary>
        [Description("未找到token")]
        Toekn_NotFind = 1002,
    }
}
