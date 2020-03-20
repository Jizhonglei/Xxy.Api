using IFramework.Base;
using Js.DomainDto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Js.Domain
{
    /// <summary>
    /// ES全文检索接口
    /// </summary>
    /// <remarks>2020.3.14 纪钟磊 创建</remarks>
    public interface IESearchService : IDependency
    {
        /// <summary>
        /// 全局索引接口
        /// </summary>
        /// <param name="request">查询条件</param>
        /// <returns>对应json数据</returns>
        /// <remarks>2020.3.14 纪钟磊 创建</remarks>
        ImproveSearchResponse GetImproveSearch(ImproveSearchRequst request);

        /// <summary>
        /// 全文检索详情接口
        /// </summary>
        /// <param name="type">检索类型</param>
        /// <param name="guid">guid</param>
        /// <returns>ES返回接口详情</returns>
        /// <remarks>2020.3.14 纪钟磊 创建</remarks>
        ImproveConditionResponse GetImproveByConditon(string type, string guid);
    }
}
