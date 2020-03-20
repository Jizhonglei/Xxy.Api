using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Js.DomainDto.Base
{
    /// <summary>
    /// 请求基类
    /// </summary>
    public class BaseRequest<T>
    {

        /// <summary>
        /// 请求参数
        /// </summary>
        public T Request { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }

        /// <summary>
        /// 登录凭据
        /// </summary>
        public string Token { get; set; }
    }
}
