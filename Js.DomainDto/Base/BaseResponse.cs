using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Js.DomainDto.Base
{
    /// <summary>
    /// 返回基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
   public class BaseResponse<T>
    {
        /// 编码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 是否处理成功
        /// </summary>
        public bool IsSucceed { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string Err { get; set; }

        /// <summary>
        /// 返回客户端数据
        /// </summary>
        public T Data { get; set; }
    }
}
