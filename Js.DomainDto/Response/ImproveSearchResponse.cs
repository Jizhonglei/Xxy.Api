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
    public class ImproveSearchResponse : BaseResponse<ImproveSearchResponse>
    {
        public string Term { get; set; }

        public int Size { get; set; }

        public int Page { get; set; }

        public int Total { get; set; }

        public List<FgTestR3> Rows { get; set; }
    }
}
