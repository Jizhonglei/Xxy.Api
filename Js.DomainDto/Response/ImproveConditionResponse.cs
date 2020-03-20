using Js.DomainDto.Base;
using Js.DomainDto.Enum;
using Js.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Js.DomainDto.Response
{
    /// <summary>
    /// 全局文章检索
    /// </summary>
    /// <remarks>纪钟磊 2020.3.13 创建</remarks>
    public class ImproveConditionResponse : BaseResponse<ImproveConditionResponse>
    {
        public AlTestR3 alTestR3 { get; set; }
    }
}
