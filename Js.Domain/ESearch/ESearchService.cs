using IFramework.IdHelper;
using IFramework.Utility.Extension;
using IFramework.Utility.Helper;
using Js.DomainDto.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Js.Domain
{
    /// <summary>
    /// ES全文检索接口实现
    /// </summary>
    /// <remarks>2020.3.14 纪钟磊 创建</remarks>
    public class ESearchService : BaseService, IESearchService
    {
        private readonly string domain = ConfigurationManager.AppSettings["domain"].ToString();

        public ESearchService(IDbSession dbSession) : base(dbSession)
        {

        }

        /// <summary>
        /// 全文检索详情接口
        /// </summary>
        /// <param name="type">检索类型</param>
        /// <param name="guid">guid</param>
        /// <returns>ES返回接口详情</returns>
        /// <remarks>2020.3.14 纪钟磊 创建</remarks>
        public ImproveConditionResponse GetImproveByConditon(string type, string guid)
        {
            ImproveConditionResponse response = new ImproveConditionResponse { IsSucceed = false };
            RestClient client = new RestClient(domain);
            var result = client.PostUrl($"bwbd/fg/article/detail?type={type}&guid={guid}", "");
            ImproveConditionResponse ret = JsonConvert.DeserializeObject<ImproveConditionResponse>(result);

            if (ret == null)
            {
                response.IsSucceed = false;
                response.Data = null;
                response.Err = "系统繁忙,请稍后再试！";
                return response;
            }

            ret.IsSucceed = true;
            return ret;
        }

        /// <summary>
        /// 全文检索接口
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>ES返回Json</returns>
        /// <remarks>2020.3.14 纪钟磊 创建</remarks>
        public ImproveSearchResponse GetImproveSearch(ImproveSearchRequst request)
        {
            ImproveSearchResponse response = new ImproveSearchResponse { IsSucceed = false };
            RestClient client = new RestClient(domain);
            var result = client.PostUrl("BwbdType/improveSearch", request.ToJson().ToLower());
            ImproveSearchResponse ret = JsonConvert.DeserializeObject<ImproveSearchResponse>(result);

            if (ret.Rows == null || ret.Rows.Count <= 0)
            {
                response.IsSucceed = false;
                response.Data = null;
                response.Err = "系统繁忙,请稍后再试！";
                return response;
            }

            ret.IsSucceed = true;
            return ret;
        }
    }
}
