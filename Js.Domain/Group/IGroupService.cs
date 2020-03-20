using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFramework.Base;
using Jsp.DomainDto.Group;

namespace Js.Domain
{
    public interface IGroupService: IDependency
    {
        #region 添加小组

        /// <summary>
        /// /添加小组
        /// </summary>
        /// <param name="requestModel">请求模型</param>
        /// <param name="labYearCacheKey"></param>
        /// <param name="isCompetition"></param>
        /// <returns></returns>
        Result<long> Add(GroupBaseResponse requestModel);

        #endregion

        #region 修改小组

        /// <summary>
        /// /修改小组
        /// </summary>
        /// <param name="id">小组id</param>
        /// <param name="requestModel">请求模型</param>
        /// <returns></returns>
        Result Edit(GroupBaseResponse requestModel);

        #endregion

        #region 删除小组

        /// <summary>
        /// 删除小组
        /// </summary>
        /// <param name="ids">小组id集合</param>
        /// <returns></returns>
        Result Delete(params long[] ids);

        #endregion

        #region 查询小组

        /// <summary>
        /// 根据Id查询小组信息
        /// </summary>
        /// <param name="id">小组Id</param>
        /// <returns></returns>
        Result<GroupBaseResponse> GetById(long id);

        /// <summary>
        /// 根据实验Id查找小组信息
        /// </summary>
        /// <param name="id">实验Id</param>
        /// <returns></returns>
        Result<List<GroupBaseResponse>> GetAllByLabId(long id);

        /// <summary>
        /// 根据当前实验查询所有小组信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Result<List<GroupBaseResponse>> GetAllByCurrentLab(GroupBaseResponse request);

        #endregion
    }
}
